Imports SP.DatabaseAccess
Imports System.Text
Imports SP.Infrastructure
Imports SP.DatabaseAccess.ESRUtility.DataObjects

Namespace ESRUtility

	Partial Public Class ESRUtilityDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IESRUtilityDatabaseAccess

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

#End Region ' Constructor


#Region "Public Methods"

		Public Function LoadBankData(mdNr As Integer) As IList(Of DataObjects.BankData) Implements IESRUtilityDatabaseAccess.LoadBankData
			Dim bankDataList As IList(Of DataObjects.BankData) = Nothing

			Dim sql As String

			sql = "SELECT ID, RecNr, MD_ID, Swift, ESRIBAN1, ESRIban2, KontoESR1, KontoESR2, BankName, asstandard FROM MD_ESRDTA "
			sql &= "WHERE ModulArt = 1 And MDNr = @MDNr And (MD_ID <> '' Or MD_ID Is not Null) "
			sql &= "Order By asstandard Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
			Try
				If reader IsNot Nothing Then
					bankDataList = New List(Of DataObjects.BankData)
					While reader.Read
						Dim bankData = New DataObjects.BankData With {
							.ID = SafeGetInteger(reader, "ID", Nothing),
							.RecNr = SafeGetInteger(reader, "RecNr", Nothing),
							.ESRCustomerID = SafeGetString(reader, "MD_ID"),
							.Swift = SafeGetString(reader, "Swift"),
							.ESRIban1 = SafeGetString(reader, "ESRIBAN1"),
							.ESRIban2 = SafeGetString(reader, "ESRIban2"),
							.KontoESR1 = SafeGetString(reader, "KontoESR1"),
							.KontoESR2 = SafeGetString(reader, "KontoESR2"),
							.BankName = SafeGetString(reader, "BankName"),
							.AsStandard = SafeGetBoolean(reader, "asstandard", False)
						}

						bankDataList.Add(bankData)
					End While

				End If


			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return bankDataList
		End Function

		Public Function LoadMandantYearData() As IList(Of DataObjects.MandantYearData) Implements IESRUtilityDatabaseAccess.LoadMandantYearData
			Dim mandantYearDataList As IList(Of DataObjects.MandantYearData) = Nothing

			Dim sql As String = "Select Jahr, COUNT(*) AS Count FROM Mandanten GROUP BY Jahr ORDER BY Jahr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
			Try
				If reader IsNot Nothing Then
					mandantYearDataList = New List(Of DataObjects.MandantYearData)
					While reader.Read
						Dim mandantYearData = New DataObjects.MandantYearData With {
				  .Jahr = SafeGetString(reader, "Jahr"),
				  .Count = SafeGetInteger(reader, "Count", 0)
				}
						mandantYearDataList.Add(mandantYearData)
					End While
				End If
			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return mandantYearDataList
		End Function

		Public Function LoadReData(ByVal mdNr As Integer, ByVal kdNr As Integer?, ByVal reNr As Integer?) As ReData Implements IESRUtilityDatabaseAccess.LoadReData
			Dim reData As ReData = Nothing

			Dim sql As String
			sql = "SELECT TOP 1 RENR, KDNR, Fak_Dat, Currency, BetragInk "
			sql &= ", MWST1, Bezahlt, Gebucht, FkSoll, R_Name1 "
			sql &= "FROM RE "
			sql &= "WHERE "
			sql &= "MDNR = @mdNr "
			sql &= "And (@kdNr = 0 Or KDNR = @kdNr) "
			sql &= "And (@reNr = 0 Or RENR = @reNr)"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("kdNr", ReplaceMissing(kdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("reNr", ReplaceMissing(reNr, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
			Try
				If reader IsNot Nothing Then
					If reader.Read Then
						reData = New DataObjects.ReData With {
								.RENR = SafeGetInteger(reader, "RENR", 0),
								.KDNR = SafeGetInteger(reader, "KDNR", 0),
								.CustomerName = SafeGetString(reader, "R_Name1"),
								.Fak_Dat = SafeGetDateTime(reader, "Fak_Dat", Nothing),
								.Currency = SafeGetString(reader, "Currency"),
								.BetragInk = SafeGetDecimal(reader, "BetragInk", 0D),
								.MWST1 = SafeGetDecimal(reader, "MWST1", 0D),
								.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0D),
								.Gebucht = SafeGetBoolean(reader, "Gebucht", False),
								.FkSoll = SafeGetInteger(reader, "FkSoll", 1)
							}
					End If
				End If
			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return reData

		End Function


		Public Function AddESRDataToPayment(ByVal mdNr As Integer, ByVal data As EsrRecord, ByVal zeNumberOffset As Integer) As Boolean Implements IESRUtilityDatabaseAccess.AddESRDataToPayment
			Dim success = True

			Dim sql As String

			sql = "[Create ZE With ESRData]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("ZENumberOffset", zeNumberOffset))
			listOfParams.Add(New SqlClient.SqlParameter("reNr", data.invoiceNumber))
			listOfParams.Add(New SqlClient.SqlParameter("kdNr", data.customerNumber))

			listOfParams.Add(New SqlClient.SqlParameter("valuta", data.valutadate))
			listOfParams.Add(New SqlClient.SqlParameter("bkonto", data.bkonto))
			listOfParams.Add(New SqlClient.SqlParameter("fksoll", data.fksoll))
			listOfParams.Add(New SqlClient.SqlParameter("betrag", data.amount))
			listOfParams.Add(New SqlClient.SqlParameter("fak_dat", data.fak_date))

			listOfParams.Add(New SqlClient.SqlParameter("mwst", data.iswithtax))
			listOfParams.Add(New SqlClient.SqlParameter("currency", data.currency))
			listOfParams.Add(New SqlClient.SqlParameter("rec", Mid(data.data, 1, 127)))
			listOfParams.Add(New SqlClient.SqlParameter("vd", data.fileinfo.LastWriteTime.Date))
			listOfParams.Add(New SqlClient.SqlParameter("vt", String.Format("{0:HH:mm}", data.fileinfo.LastWriteTime)))

			listOfParams.Add(New SqlClient.SqlParameter("month", data.valutadate.Value.Month))
			listOfParams.Add(New SqlClient.SqlParameter("dikey", data.dikey))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", data.createdfrom))

			listOfParams.Add(New SqlClient.SqlParameter("amountdecision", data.amountDecision))
			listOfParams.Add(New SqlClient.SqlParameter("isinvoicefinished", data.isinvoicefinished))

			listOfParams.Add(New SqlClient.SqlParameter("IsFinished", data.isinvoicefinished))

			' New ID of ZE
			Dim newIdParameter = New SqlClient.SqlParameter("@NewZEID", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			' New ZENr
			Dim newZENrParameter = New SqlClient.SqlParameter("@ZENR", SqlDbType.Int)
			newZENrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newZENrParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing AndAlso Not newZENrParameter.Value Is Nothing Then
				data.paymentID = CType(newIdParameter.Value, Integer)
				data.paymentNumber = CType(newZENrParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		Public Function AddESRDataToESRTable(ByVal mdNr As Integer, ByVal data As EsrRecord) As Boolean Implements IESRUtilityDatabaseAccess.AddESRDataToESRTable
			Dim success = True

			Dim sql As String

			sql = "[Add ESRRecord To ESRTable]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("reNr", ReplaceMissing(data.invoiceNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kdNr", ReplaceMissing(data.customerNumber, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("PayedAmount", ReplaceMissing(data.PayedAmount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("betrag", ReplaceMissing(data.amount, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("rec", Mid(data.data, 1, 127)))
			listOfParams.Add(New SqlClient.SqlParameter("vd", data.fileinfo.LastWriteTime.Date))
			listOfParams.Add(New SqlClient.SqlParameter("vt", String.Format("{0:HH:mm}", data.fileinfo.LastWriteTime)))

			listOfParams.Add(New SqlClient.SqlParameter("dikey", ReplaceMissing(data.dikey, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(data.createdfrom, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("amountdecision", ReplaceMissing(data.amountDecision, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Public Function AddESRFileDataToDiskInfo(ByVal mdNr As Integer, ByVal data As EsrRecord, ByVal filebytes() As Byte) As Boolean Implements IESRUtilityDatabaseAccess.AddESRFileDataToDiskInfo
			Dim success = True

			Dim sql As String

			sql = "Insert Into DiskInfo (MDNr, VD, VT, Betrag, nTa, eDat, DiskKey, CreatedOn, CreatedFrom, ESRFileContent) Values ("
			sql &= "@MDNr, @VD, @VT, @Betrag, @nTa, @eDat, @DiskKey, GetDate(), @CreatedFrom, @filebytes)"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			listOfParams.Add(New SqlClient.SqlParameter("vd", String.Format("{0:d}", data.fileinfo.LastWriteTime)))
			listOfParams.Add(New SqlClient.SqlParameter("vt", String.Format("{0:HH:mm}", data.fileinfo.LastWriteTime)))

			listOfParams.Add(New SqlClient.SqlParameter("Betrag", ReplaceMissing(data.bookingamountsum, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("nTa", ReplaceMissing(data.bookingcount, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("eDat", String.Format("{0:d}", data.fileinfo.LastWriteTime)))

			listOfParams.Add(New SqlClient.SqlParameter("DiskKey", data.dikey))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", data.createdfrom))

			listOfParams.Add(New SqlClient.SqlParameter("filebytes", ReplaceMissing(filebytes, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Public Function IsESRFileAlreadySaved(ByVal mdNr As Integer, ByVal fileinfo As System.IO.FileInfo, ByVal filebytes() As Byte) As SavedESRData Implements IESRUtilityDatabaseAccess.IsESRFileAlreadySaved
			Dim result As SavedESRData = Nothing

			Dim sql As String

			sql = "Select Top 1 CreatedOn, CreatedFrom From DiskInfo Where MDNr = @MDNr And VD = @VD And ESRFileContent = @filebytes"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("vd", String.Format("{0:d}", fileinfo.LastWriteTime)))
			listOfParams.Add(New SqlClient.SqlParameter("filebytes", ReplaceMissing(filebytes, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim data = New SavedESRData
					data.createdon = SafeGetDateTime(reader, "createdon", Nothing)
					data.createdfrom = SafeGetString(reader, "createdfrom")

					result = data

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


#End Region ' Public Methods


	End Class


End Namespace
