
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Listing.DataObjects

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess

		Function LoadInvoicesCustomerData() As IEnumerable(Of CustomerMasterData) Implements IListingDatabaseAccess.LoadInvoicesCustomerData

			Dim result As List(Of CustomerMasterData) = Nothing

			Dim sql As String

			sql = "Select "
#If DEBUG Then
			sql &= "Top 10 "
#End If
			sql &= "RE.KDNr "
			sql &= "From dbo.RE "
			sql &= "Group By RE.KDNr "
			sql &= "Order By RE.KDNr"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerMasterData)

					While reader.Read

						Dim data = New CustomerMasterData

						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)

						result.Add(data)

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

		Function LoadEmploymentAgreementData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of EmploymentAgreementData) Implements IListingDatabaseAccess.LoadEmploymentAgreementData

			Dim result As List(Of EmploymentAgreementData) = Nothing

			Dim sql As String

			sql = "SELECT "
#If DEBUG Then
			sql &= "Top 10 "
#End If
			sql &= "ES.ESNr, ES.MANr, ES.KDNr, ES.ES_Als, ES.KDZHDNr, "
			sql &= "ESL.GAVGruppe0, "
			sql &= "MA.Send2WOS As MAWOS, "
			sql &= "IsNull(MA.Sprache, 'Deutsch') As MASprache, "
			sql &= "(Convert(nvarchar(10), ES.ES_Ab, 104) + ' - ' + IsNull(convert(nvarchar(10), ES.ES_Ende, 104), '')) As Zeitraum, "
			sql &= "(MA.Nachname + ', ' + MA.Vorname) As MAName, "
			sql &= "KD.Firma1, KD.Send2WOS As KDWOS, IsNull(KD.Sprache, 'Deutsch') As KDSprache "
			sql &= "FROM dbo.ES "
			sql &= "Left Join dbo.ESLohn ESL On ES.ESNr = ESL.ESNr And ESL.Aktivlodaten = 1 "
			sql &= "Left Join dbo.Mitarbeiter MA On ES.MANr = MA.MANr "
			sql &= "Left Join dbo.Kunden KD On ES.KDNr = KD.KDNr "

			sql &= "Where "
			sql &= "(@mdnr = 0 Or ES.MDNr = @mdnr) "
			sql &= String.Format("AND (@year = 0 OR ( ES.ES_Ende >= '01.01.{0}' Or ES.ES_Ende Is Null) And ES.ES_Ab <= '31.12.{0}' ) ", year)

			sql &= "Order By ES.ESNr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmploymentAgreementData)

					While reader.Read

						Dim data = New EmploymentAgreementData

						data.EmploymentNumber = SafeGetInteger(reader, "ESNr", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						data.CResponsibleNumber = SafeGetInteger(reader, "KDZHDNr", 0)

						data.EmployeeWOS = False
						data.CustomerWOS = False

						data.EmployeeLanguage = SafeGetString(reader, "MASprache").Substring(0, 1).ToUpper
						data.CustomerLanguage = SafeGetString(reader, "KDSprache").Substring(0, 1).ToUpper


						result.Add(data)

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

		Function LoadEmploymentReportsForGivenTimeperiodData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of ListingESListData) Implements IListingDatabaseAccess.LoadEmploymentReportsForGivenTimeperiodData

			Dim result As List(Of ListingESListData) = Nothing

			Dim sql As String

			sql = "[Load Employments For Export Employment List]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("FirstYear", ReplaceMissing(year, 2000)))
			listOfParams.Add(New SqlClient.SqlParameter("lastYear", ReplaceMissing(year, 2022)))

			'listOfParams.Add(New SqlClient.SqlParameter("Firstmonth", 1))
			'listOfParams.Add(New SqlClient.SqlParameter("lastMonth", 12))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ListingESListData)

					While reader.Read

						Dim data As New ListingESListData

						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)
						data.MANr = SafeGetInteger(reader, "MANr", 0)

						data.ahv_nr = SafeGetString(reader, "AHV_Nr_New")
						data.employeelastname = SafeGetString(reader, "Nachname")
						data.employeefirstname = SafeGetString(reader, "Vorname")

						data.customername = SafeGetString(reader, "Firma1")
						data.GebDat = SafeGetDateTime(reader, "GebDat", Nothing)
						data.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
						data.ES_Ende = SafeGetDateTime(reader, "ES_Ende", Nothing)

						data.MABeruf = SafeGetString(reader, "MABeruf")
						data.UID = SafeGetString(reader, "UID")
						data.NogaCode = SafeGetString(reader, "NogaCode")
						data.KDOrt = SafeGetString(reader, "KDOrt")

						data.ESOrt = SafeGetString(reader, "ESOrt")
						data.GAVNumber = SafeGetInteger(reader, "GAVNumber", 0)
						data.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0")
						data.ES_Als = SafeGetString(reader, "ES_Als")
						data.Einstufung = SafeGetString(reader, "Einstufung")
						data.GAVBezeichnung = SafeGetString(reader, "GAVBezeichnung")
						data.GAVInfo_String = SafeGetString(reader, "GAVInfo_String")

						data.Grundlohn = SafeGetDecimal(reader, "Grundlohn", 0)
						data.Ferien = SafeGetDecimal(reader, "Ferien", 0)
						data.FerienProz = SafeGetDecimal(reader, "FerienProz", 0)
						data.Feier = SafeGetDecimal(reader, "Feier", 0)
						data.FeierProz = SafeGetDecimal(reader, "FeierProz", 0)
						data.Lohn13 = SafeGetDecimal(reader, "Lohn13", 0)
						data.Lohn13Proz = SafeGetDecimal(reader, "Lohn13Proz", 0)

						data.Bruttolohn = SafeGetDecimal(reader, "Bruttolohn", 0)
						data.ESSpesen = SafeGetDecimal(reader, "TotalBetragSpesen", 0)
						data.ESStunden = SafeGetDecimal(reader, "KumulierteStundenEinsatz", 0)


						result.Add(data)

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

		Function LoadReportNumberData(ByVal mdnr As Integer, ByVal year As Integer?, ByVal WithScans As Boolean) As IEnumerable(Of Integer) Implements IListingDatabaseAccess.LoadReportNumberData

			Dim result As List(Of Integer) = Nothing

			Dim sql As String

			sql = "Select "
#If DEBUG Then
			sql &= "Top 10 "
#End If
			sql &= "RP.RPNr "
			sql &= "From dbo.RP "
			sql &= "Where "
			sql &= "(@mdnr = 0 Or RP.MDNr = @mdnr) "
			sql &= "AND (@year = 0 OR Convert(Int, RP.Jahr) = @year) "

			If WithScans Then
				sql &= "AND Exists (Select S.RPNr From dbo.RP_ScanDoc S Where S.RPNr = RP.RPNr AND S.DocScan Is Not Null) "
			End If

			sql &= "Order By RP.RPNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Integer)

					While reader.Read

						result.Add(SafeGetInteger(reader, "RPNr", 0))

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

		Function LoadReportScanData(ByVal rpNr As Integer?, ByVal id As Integer?, ByVal includeFileBytes As Boolean) As IEnumerable(Of ReportScanData) Implements IListingDatabaseAccess.LoadReportScanData

			Dim result As List(Of ReportScanData) = Nothing

			Dim sql As String

			sql = "Select "
#If DEBUG Then
			sql &= "Top 10 "
#End If
			sql &= "s.ID "
			sql &= ", s.MANr "
			sql &= ", s.KDNr "
			sql &= ", s.ESNr "
			sql &= ", s.RPNr "
			sql &= ", s.RPLNr "

			If includeFileBytes Then
				sql &= ", s.DocScan "
			End If

			sql &= "From dbo.RP_ScanDoc s "
			sql &= "Where "
			sql &= "(@rpNr = 0 OR s.RPNr = @RPNr) "
			sql &= "AND (s.DocScan Is Not Null) "
			sql &= "AND (@id = 0 OR s.ID = @id) "
			sql &= "Order By s.RPNr, s.ID"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(rpNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("id", ReplaceMissing(id, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ReportScanData)

					While reader.Read
						Dim data = New ReportScanData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						data.EmploymentNumber = SafeGetInteger(reader, "ESNr", 0)
						data.ReportNumber = SafeGetInteger(reader, "RPNr", 0)
						data.RPLNr = SafeGetInteger(reader, "RPLNr", 0)

						If includeFileBytes Then
							data.FileBytes = SafeGetByteArray(reader, "DocScan")
						End If


						result.Add(data)

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



		Function LoadInvoiceForExportData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of Integer) Implements IListingDatabaseAccess.LoadInvoiceForExportData

			Dim result As List(Of Integer) = Nothing

			Dim sql As String

			sql = "Select "
#If DEBUG Then
			sql &= "Top 10 "
#End If
			sql &= "RE.RENr "
			sql &= "From dbo.RE "
			sql &= "Where "
			sql &= "(@mdnr = 0 Or RE.MDNr = @mdNr) "
			sql &= "AND (@year = 0 Or Year(RE.Fak_Dat) = @year) "
			sql &= "Order By RE.RENr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Integer)

					While reader.Read

						result.Add(SafeGetInteger(reader, "RENr", 0))

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

		Function LoadInvoiceEvaluationData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of Invoice.DataObjects.Invoice) Implements IListingDatabaseAccess.LoadInvoiceEvaluationData

			Dim result As List(Of Invoice.DataObjects.Invoice) = Nothing

			Dim sql As String

			sql = "[Load Invoice For Export Evaluation List]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Invoice.DataObjects.Invoice)

					While reader.Read

						Dim data As New Invoice.DataObjects.Invoice

						data.ReNr = SafeGetInteger(reader, "RENR", Nothing)
						data.KdNr = SafeGetInteger(reader, "KDNR", Nothing)
						data.Art = SafeGetString(reader, "ART", "")
						data.KST = SafeGetString(reader, "KST", "")
						data.Lp = SafeGetInteger(reader, "LP", Nothing)
						data.FakDat = SafeGetDateTime(reader, "FAK_DAT", Nothing)
						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.MWST1 = SafeGetDecimal(reader, "MWST1", 0)
						data.MWSTProz = SafeGetDecimal(reader, "MWSTProz", Nothing)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)
						data.SKonto = SafeGetDecimal(reader, "SKonto", Nothing)
						data.FSKonto = SafeGetDecimal(reader, "FSKonto", Nothing)
						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)
						data.Mahncode = SafeGetString(reader, "Mahncode", "")
						data.MA0 = SafeGetDateTime(reader, "MA0", Nothing)
						data.MA1 = SafeGetDateTime(reader, "MA1", Nothing)
						data.MA2 = SafeGetDateTime(reader, "MA2", Nothing)
						data.MA3 = SafeGetDateTime(reader, "MA3", Nothing)
						data.Gebucht = SafeGetBoolean(reader, "Gebucht", False)
						data.FKSoll = SafeGetInteger(reader, "FKSoll", Nothing)
						data.FKHaben0 = SafeGetInteger(reader, "FKHaben0", Nothing)
						data.FKHaben1 = SafeGetInteger(reader, "FKHaben1", Nothing)
						data.RName1 = SafeGetString(reader, "R_Name1", "")
						data.RName2 = SafeGetString(reader, "R_Name2", "")
						data.RName3 = SafeGetString(reader, "R_Name3", "")
						data.RZHD = SafeGetString(reader, "R_ZHD", "")
						data.RPostfach = SafeGetString(reader, "R_Postfach", "")
						data.RStrasse = SafeGetString(reader, "R_Strasse", "")
						data.RLand = SafeGetString(reader, "R_Land", "")
						data.RPLZ = SafeGetString(reader, "R_PLZ", "")
						data.ROrt = SafeGetString(reader, "R_Ort", "")
						data.ReMail = SafeGetString(reader, "REEMail", "")
						data.Zahlkond = SafeGetString(reader, "Zahlkond", "")
						data.RefNr = SafeGetString(reader, "RefNr", "")
						data.RefFootNr = SafeGetString(reader, "RefFootNr", "")
						data.ESRID = SafeGetString(reader, "ESRID", "")
						data.ESRKonto = SafeGetString(reader, "ESRKonto", "")
						data.MWSTNr = SafeGetString(reader, "MWSTNr", "")
						data.KontoNr = SafeGetString(reader, "KontoNr", "")
						data.REKST1 = SafeGetString(reader, "REKST1", "")
						data.REKST2 = SafeGetString(reader, "REKST2", "")
						data.PrintedDate = SafeGetString(reader, "PrintedDate", "")
						data.GebuchtAm = SafeGetDateTime(reader, "GebuchtAm", Nothing)
						data.ZEInfo = SafeGetString(reader, "ZEInfo", "")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", "")
						data.RAbteilung = SafeGetString(reader, "R_Abteilung", "")
						data.EsEinstufung = SafeGetString(reader, "ES_Einstufung", "")
						data.KDBranche = SafeGetString(reader, "KDBranche", "")
						data.ESRBankName = SafeGetString(reader, "ESR BankName", "")
						data.ESRBankAdresse = SafeGetString(reader, "ESR BankAdresse", "")
						data.EsrSwift = SafeGetString(reader, "ESR_Swift", "")
						data.EsrIBAN1 = SafeGetString(reader, "ESR_IBAN1", "")
						data.EsrIBAN2 = SafeGetString(reader, "ESR_IBAN2", "")
						data.EsrIBAN3 = SafeGetString(reader, "ESR_IBAN3", "")
						data.EsrBcNr = SafeGetString(reader, "ESR_BcNr", "")
						data.Art2 = SafeGetString(reader, "Art_2", "")
						data.MahnStopUntil = SafeGetDateTime(reader, "MahnStopUntil", Nothing)
						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						data.PaymentExtraAmounts = LoadPaymentExtraData(mdnr, data.ReNr)


						result.Add(data)

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

		Function LoadPaymentEvaluationData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of Invoice.DataObjects.PaymentMasterData) Implements IListingDatabaseAccess.LoadPaymentEvaluationData

			Dim result As List(Of Invoice.DataObjects.PaymentMasterData) = Nothing

			Dim sql As String

			sql = "[Load Payment For Export Evaluation List]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Invoice.DataObjects.PaymentMasterData)

					While reader.Read

						Dim data As New Invoice.DataObjects.PaymentMasterData

						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						data.RENR = SafeGetInteger(reader, "RENR", Nothing)
						data.ZENr = SafeGetInteger(reader, "ZENr", Nothing)
						data.KDNR = SafeGetInteger(reader, "KDNR", Nothing)
						data.FakDate = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						data.VDate = SafeGetDateTime(reader, "V_Date", Nothing)
						data.BDate = SafeGetDateTime(reader, "B_Date", Nothing)
						data.Currency = SafeGetString(reader, "Currency", "")
						data.InvoiceAmount = SafeGetDecimal(reader, "InvoiceAmount", Nothing)
						data.Amount = SafeGetDecimal(reader, "BETRAG", Nothing)
						data.MWSTAmount = SafeGetDecimal(reader, "MWST-Betrag", Nothing)
						data.InvoiceTaxPercent = SafeGetDecimal(reader, "InvoiceTaxPercent", Nothing)
						data.FBMONAT = SafeGetInteger(reader, "FBMonat", Nothing)
						data.FBDAT = SafeGetDateTime(reader, "FBDat", Nothing)
						data.FKSOLL = SafeGetInteger(reader, "FKSoll", Nothing)
						data.FKHABEN = SafeGetInteger(reader, "FKHABEN", Nothing)
						data.MWST = SafeGetBoolean(reader, "MWST", False)
						data.DiskInfo = SafeGetString(reader, "DiskInfo", "")
						data.ZeInfo = SafeGetString(reader, "ZEInfo", "")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", "")
						

						result.Add(data)

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


#Region "private mehthods"

		Private Function LoadPaymentExtraData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of Invoice.DataObjects.PaymentExtraData)
			Dim result As List(Of Invoice.DataObjects.PaymentExtraData) = Nothing

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

					result = New List(Of Invoice.DataObjects.PaymentExtraData)

					While reader.Read

						Dim data = New Invoice.DataObjects.PaymentExtraData

						data.ZENr = SafeGetInteger(reader, "zenr", Nothing)
						data.FKSollKonto = SafeGetInteger(reader, "FKSOLL", Nothing)
						data.Amount = SafeGetDecimal(reader, "Betrag", Nothing)
						data.ValutaDate = SafeGetDateTime(reader, "V_Date", Nothing)
						data.BookingDate = SafeGetDateTime(reader, "B_Date", Nothing)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", "")


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
