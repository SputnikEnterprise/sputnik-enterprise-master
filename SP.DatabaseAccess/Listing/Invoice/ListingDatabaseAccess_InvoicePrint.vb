
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		'Function LoadInvoiceData(ByVal mdNr As Integer, ByVal invoiceNumbers As List(Of Integer), ByVal orderbyEnum As OrderByValue, ByVal WOSValueEnum As WOSSearchValue) As IEnumerable(Of InvoiceData) Implements IListingDatabaseAccess.LoadInvoiceData
		'Function LoadInvoiceData(ByVal searchData As InvoicePrintSearchConditionData) As IEnumerable(Of InvoiceData) Implements IListingDatabaseAccess.LoadInvoiceData

		'	Dim result As List(Of InvoiceData) = Nothing

		'	Dim invoiceNumbersBuffer As String = String.Empty
		'	Dim invoiceStartNumber As String = String.Empty

		'	If searchData.InvoiceNumbers.Count > 0 AndAlso searchData.InvoiceNumbers.Count = 2 AndAlso searchData.InvoiceNumbers(1) = 0 Then
		'		invoiceStartNumber = searchData.InvoiceNumbers(0)
		'	Else

		'		For Each number In searchData.InvoiceNumbers

		'			invoiceNumbersBuffer = invoiceNumbersBuffer & IIf(invoiceNumbersBuffer <> "", ", ", "") & number

		'		Next
		'	End If

		'	Dim sql As String

		'	'sql = "Select RE.MDNr"
		'	'sql &= ",RE.ID"
		'	'sql &= ",RE.Art"
		'	'sql &= ",RE.RENr"
		'	'sql &= ",RE.KDNr"
		'	'sql &= ",RE.RefNr"
		'	'sql &= ",RE.Fak_Dat"
		'	'sql &= ",RE.BetragInk"
		'	'sql &= ",RE.Bezahlt"
		'	'sql &= ",RE.R_Name1"
		'	'sql &= ",RE.CreatedOn"
		'	'sql &= ",RE.CreatedFrom"
		'	'sql &= ",IsNull(RE.REDoc_Guid, '') DocGuid"

		'	'sql &= ",IsNull(KD.Sprache, 'D') Sprache"
		'	'sql &= ",IsNull(Convert(Int, KD.AnzKopien), 1) AnzKopien"
		'	'sql &= ",IsNull(KD.Send2WOS, 0) As Send2WOS"
		'	'sql &= ",IsNull(KD.Transfered_Guid, '') CustomerGuid"
		'	'sql &= ",CONVERT(BIT,("
		'	'sql &= " CASE"
		'	'sql &= " WHEN ISNULL(kd.OPVersand, '') LIKE '99%' THEN 1"
		'	'sql &= " ELSE 0"
		'	'sql &= " END"
		'	'sql &= "), 0) PrintWithReport"

		'	'sql &= ",CONVERT(BIT,("
		'	'sql &= " CASE"
		'	'sql &= " WHEN ISNULL(RE.Art, '') = 'G' THEN"
		'	'sql &= " ( CASE"
		'	'sql &= " WHEN EXISTS (Select Top 1 I.ID From RE_Ind I Where I.RENr = RE.RENr) THEN 0"
		'	'sql &= " ELSE 1"
		'	'sql &= " END)"

		'	'sql &= " ELSE 0"
		'	'sql &= " END"
		'	'sql &= "), 0) CreditInvoiceAutomated"

		'	'sql &= " From RE "
		'	'sql &= " Left Join Kunden KD On RE.KDNr = KD.KDNr "

		'	'sql &= " Where RE.MDNr = @mdNr"
		'	'sql &= " And RE.BetragInk <> 0"

		'	If Not String.IsNullOrWhiteSpace(invoiceNumbersBuffer) Then
		'		sql &= " And RE.RENr In (" & invoiceNumbersBuffer & ")"
		'	ElseIf Not String.IsNullOrWhiteSpace(invoiceStartNumber) Then
		'		invoiceNumbersBuffer = String.Empty
		'		sql &= " And RE.RENr >= " & invoiceStartNumber
		'	End If

		'	Dim customerWOS As Integer?
		'	Select Case searchData.WOSValueEnum
		'		Case WOSSearchValue.SearchNOTWOSCustomer
		'			customerWOS = 0
		'			sql &= " And IsNull(KD.Send2WOS, 0) = 0"

		'		Case WOSSearchValue.SearchWOSCustomer
		'			customerWOS = 1
		'			sql &= " And IsNull(KD.Send2WOS, 0) = 1"

		'	End Select

		'	Select Case searchData.OrderByEnum
		'		Case OrderByValue.OrderByCustomerName
		'			sql &= " Order By RE.R_Name1, RE.R_Ort"

		'		Case OrderByValue.OrderByInvoiceDate
		'			sql &= " Order By RE.Fak_Dat"

		'		Case Else
		'			sql &= " Order By RE.RENr"

		'	End Select
		'	sql = "[Load Invoice Data For Print]"


		'	' Parameters
		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)
		'	listOfParams.Add(New SqlClient.SqlParameter("mdNr", searchData.MDNr))
		'	listOfParams.Add(New SqlClient.SqlParameter("startRENr", invoiceStartNumber))
		'	listOfParams.Add(New SqlClient.SqlParameter("endRENr", searchData.MDNr))
		'	listOfParams.Add(New SqlClient.SqlParameter("numbers", invoiceNumbersBuffer))
		'	listOfParams.Add(New SqlClient.SqlParameter("withKDWOS", ReplaceMissing(customerWOS, DBNull.Value)))
		'	listOfParams.Add(New SqlClient.SqlParameter("orderBy", ReplaceMissing(searchData.OrderByEnum, 0)))


		'	Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

		'	Try
		'		If reader IsNot Nothing Then

		'			result = New List(Of InvoiceData)

		'			While reader.Read
		'				Dim data = New InvoiceData

		'				data.ID = SafeGetInteger(reader, "ID", 0)
		'				data.MDNr = SafeGetInteger(reader, "MDNr", 0)
		'				data.RENr = SafeGetInteger(reader, "RENr", 0)
		'				data.KDNr = SafeGetInteger(reader, "KDNr", 0)
		'				data.InvoiceDate = SafeGetDateTime(reader, "Fak_Dat", Nothing)
		'				data.Art = SafeGetString(reader, "Art")
		'				data.DocGuid = SafeGetString(reader, "DocGuid")
		'				data.CustomerGuid = SafeGetString(reader, "CustomerGuid")
		'				data.CustomerName = SafeGetString(reader, "R_Name1")
		'				data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
		'				data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)

		'				data.Language = Mid(SafeGetString(reader, "Sprache"), 1, 1).ToUpper
		'				data.NumberOfCopies = SafeGetInteger(reader, "AnzKopien", 1)
		'				data.Send2WOS = SafeGetBoolean(reader, "Send2WOS", False)
		'				data.PrintWithReport = SafeGetBoolean(reader, "PrintWithReport", False)
		'				data.CreditInvoiceAutomated = SafeGetBoolean(reader, "CreditInvoiceAutomated", False)
		'				data.RefNr = SafeGetString(reader, "RefNr")

		'				data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
		'				data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


		'				result.Add(data)

		'			End While

		'		End If

		'	Catch e As Exception
		'		m_Logger.LogError(e.ToString())
		'		result = Nothing
		'	Finally
		'		CloseReader(reader)
		'	End Try

		'	Return result

		'End Function

		Function LoadInvoiceForEMailSendingData(ByVal searchData As InvoicePrintSearchConditionData) As IEnumerable(Of InvoiceData) Implements IListingDatabaseAccess.LoadInvoiceForEMailSendingData

			Dim result As List(Of InvoiceData) = Nothing

			Dim invoiceNumbersBuffer As String = String.Empty
			Dim invoiceStartNumber As Integer

			Dim customerNumbersBuffer As String = String.Empty
			Dim customerStartNumber As Integer

			If searchData.CustomerNumbers.Count > 0 AndAlso searchData.CustomerNumbers.Count = 2 AndAlso searchData.CustomerNumbers(1) = 0 Then
				customerStartNumber = searchData.CustomerNumbers(0)

			Else

				For Each number In searchData.CustomerNumbers
					customerNumbersBuffer = customerNumbersBuffer & IIf(customerNumbersBuffer <> "", ", ", "") & number
				Next
			End If


			If searchData.InvoiceNumbers.Count > 0 AndAlso searchData.InvoiceNumbers.Count = 2 AndAlso searchData.InvoiceNumbers(1) = 0 Then
				invoiceStartNumber = searchData.InvoiceNumbers(0)

			Else

				For Each number In searchData.InvoiceNumbers
					invoiceNumbersBuffer = invoiceNumbersBuffer & IIf(invoiceNumbersBuffer <> "", ", ", "") & number
				Next
			End If


			Dim sql As String

			Dim customerWOS As Integer?
			Select Case searchData.WOSValueEnum
				Case WOSSearchValue.SearchNOTWOSCustomer
					customerWOS = 0

				Case WOSSearchValue.SearchWOSCustomer
					customerWOS = 1

			End Select

			sql = "[Load Invoice Data For Sending With EMail]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", searchData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("startRENr", invoiceStartNumber))
			listOfParams.Add(New SqlClient.SqlParameter("endRENr", DBNull.Value))
			listOfParams.Add(New SqlClient.SqlParameter("startKDNr", customerStartNumber))
			listOfParams.Add(New SqlClient.SqlParameter("endKDNr", DBNull.Value))

			listOfParams.Add(New SqlClient.SqlParameter("numbers", invoiceNumbersBuffer))
			listOfParams.Add(New SqlClient.SqlParameter("customerNumbers", customerNumbersBuffer))
			listOfParams.Add(New SqlClient.SqlParameter("withKDWOS", ReplaceMissing(customerWOS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("orderBy", ReplaceMissing(searchData.OrderByEnum, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoiceData)

					While reader.Read
						Dim data = New InvoiceData

						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.RENr = SafeGetInteger(reader, "RENr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)
						data.CustomerName = SafeGetString(reader, "R_Name1")
						data.REEmail = SafeGetString(reader, "REEmail")
						data.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)
						data.PrintWithReport = SafeGetBoolean(reader, "PrintWithReport", False)
						data.CreditInvoiceAutomated = SafeGetBoolean(reader, "CreditInvoiceAutomated", False)
						data.OneInvoicePerMail = SafeGetBoolean(reader, "OneInvoicePerMail", False)


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

		'Function LoadInvoiceData(ByVal mdNr As Integer, ByVal invoiceNumbers As Integer(), ByVal customerNumbers As Integer(), ByVal orderbyEnum As OrderByValue, ByVal WOSValueEnum As WOSSearchValue) As IEnumerable(Of InvoiceData) Implements IListingDatabaseAccess.LoadInvoiceData
		Function LoadInvoiceData(ByVal searchData As InvoicePrintSearchConditionData) As IEnumerable(Of InvoiceData) Implements IListingDatabaseAccess.LoadInvoiceData

			Dim result As List(Of InvoiceData) = Nothing

			Dim invoiceNumbersBuffer As String = String.Empty
			Dim invoiceStartNumber As Integer

			Dim customerNumbersBuffer As String = String.Empty
			Dim customerStartNumber As Integer

			If searchData.CustomerNumbers.Count > 0 AndAlso searchData.CustomerNumbers.Count = 2 AndAlso searchData.CustomerNumbers(1) = 0 Then
				customerStartNumber = searchData.CustomerNumbers(0)

			Else

				For Each number In searchData.CustomerNumbers
					customerNumbersBuffer = customerNumbersBuffer & IIf(customerNumbersBuffer <> "", ", ", "") & number
				Next
			End If


			If searchData.InvoiceNumbers.Count > 0 AndAlso searchData.InvoiceNumbers.Count = 2 AndAlso searchData.InvoiceNumbers(1) = 0 Then
				invoiceStartNumber = searchData.InvoiceNumbers(0)

			Else

				For Each number In searchData.InvoiceNumbers
					invoiceNumbersBuffer = invoiceNumbersBuffer & IIf(invoiceNumbersBuffer <> "", ", ", "") & number
				Next
			End If


			Dim sql As String

			'If Not String.IsNullOrWhiteSpace(invoiceNumbersBuffer) Then
			'	'sql = " And RE.RENr In (" & invoiceNumbersBuffer & ")"
			'ElseIf Not String.IsNullOrWhiteSpace(invoiceStartNumber) Then
			'	'invoiceNumbersBuffer = String.Empty
			'	'sql &= " And RE.RENr >= " & invoiceStartNumber
			'End If

			'If Not String.IsNullOrWhiteSpace(customerNumbersBuffer) Then
			'	'sql &= " And RE.KDNr In (" & customerNumbersBuffer & ")"
			'ElseIf Not String.IsNullOrWhiteSpace(customerStartNumber) Then
			'	'customerNumbersBuffer = String.Empty
			'	'sql &= " And RE.KDNr >= " & customerStartNumber
			'End If

			Dim customerWOS As Integer?
			Select Case searchData.WOSValueEnum
				Case WOSSearchValue.SearchNOTWOSCustomer
					customerWOS = 0
					'sql &= " And IsNull(KD.Send2WOS, 0) = 0"

				Case WOSSearchValue.SearchWOSCustomer
					customerWOS = 1
					'sql &= " And IsNull(KD.Send2WOS, 0) = 1"

			End Select

			'Select Case searchData.OrderByEnum
			'	Case OrderByValue.OrderByCustomerName
			'		'sql &= " Order By RE.R_Name1, RE.R_Ort"

			'	Case OrderByValue.OrderByInvoiceDate
			'		'sql &= " Order By RE.Fak_Dat"

			'	Case Else
			'		'sql &= " Order By RE.RENr"

			'End Select

			sql = "[Load Invoice Data For Print]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", searchData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("startRENr", invoiceStartNumber))
			listOfParams.Add(New SqlClient.SqlParameter("endRENr", DBNull.Value))
			listOfParams.Add(New SqlClient.SqlParameter("startKDNr", customerStartNumber))
			listOfParams.Add(New SqlClient.SqlParameter("endKDNr", DBNull.Value))

			listOfParams.Add(New SqlClient.SqlParameter("numbers", invoiceNumbersBuffer)) ' ReplaceMissing(invoiceNumbersBuffer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerNumbers", customerNumbersBuffer)) ' ReplaceMissing(customerNumbersBuffer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("withKDWOS", ReplaceMissing(customerWOS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("orderBy", ReplaceMissing(searchData.OrderByEnum, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

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
						data.REEmail = SafeGetString(reader, "REEmail")
						data.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)
						data.DocGuid = SafeGetString(reader, "DocGuid")
						data.CustomerGuid = SafeGetString(reader, "CustomerGuid")
						data.CustomerName = SafeGetString(reader, "R_Name1")
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)

						data.Language = Mid(SafeGetString(reader, "Sprache"), 1, 1).ToUpper
						data.NumberOfCopies = SafeGetInteger(reader, "AnzKopien", 1)
						data.Send2WOS = SafeGetBoolean(reader, "Send2WOS", False)
						data.PrintWithReport = SafeGetBoolean(reader, "PrintWithReport", False)
						data.CreditInvoiceAutomated = SafeGetBoolean(reader, "CreditInvoiceAutomated", False)
						data.OneInvoicePerMail = SafeGetBoolean(reader, "OneInvoicePerMail", False)
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

		Function LoadAssignedAutomatedInvoicePrintData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer, ByVal customerLanguage As String) As IEnumerable(Of InvoicePrintData) Implements IListingDatabaseAccess.LoadAssignedAutomatedInvoicePrintData

			Dim result As List(Of InvoicePrintData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "[Get Automated InvoiceData For Print]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerLanguage", ReplaceMissing(customerLanguage, "D")))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoicePrintData)

					While reader.Read
						Dim data = New InvoicePrintData

						data.Art = SafeGetString(reader, "Art", String.Empty)
						data.RENr = SafeGetInteger(reader, "RENr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)

						data.KST = SafeGetString(reader, "KST", String.Empty)
						data.Fak_Dat = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						data.ValutaData = SafeGetDateTime(reader, "ValutaData", Nothing)
						data.Currency = SafeGetString(reader, "Currency", String.Empty)

						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.MwSt1 = SafeGetDecimal(reader, "MwSt1", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)

						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)

						data.R_Name1 = SafeGetString(reader, "R_Name1", String.Empty)
						data.R_Name2 = SafeGetString(reader, "R_Name2", String.Empty)
						data.R_Name3 = SafeGetString(reader, "R_Name3", String.Empty)
						data.R_ZHD = SafeGetString(reader, "R_ZHD", String.Empty)
						data.R_Postfach = SafeGetString(reader, "R_Postfach", String.Empty)
						data.R_Strasse = SafeGetString(reader, "R_Strasse", String.Empty)
						data.R_Abteilung = SafeGetString(reader, "R_Abteilung", String.Empty)

						data.MwStProz = SafeGetDecimal(reader, "MwStProz", 8)

						data.PLZOrt = SafeGetString(reader, "PLZOrt", String.Empty)
						data.R_Land = SafeGetString(reader, "R_Land", String.Empty)
						data.RefNr = SafeGetString(reader, "RefNr", String.Empty)
						data.RefFootNr = SafeGetString(reader, "RefFootNr", String.Empty)
						data.ESRID = SafeGetString(reader, "ESRID", String.Empty)
						data.ESRKonto = SafeGetString(reader, "ESRKonto", String.Empty)
						data.KontoNr = SafeGetString(reader, "KontoNr", String.Empty)
						data.btrFr = SafeGetInteger(reader, "btrFr", 0)
						data.BtrRP = SafeGetInteger(reader, "BtrRP", 0)
						data.ZahlKond = SafeGetString(reader, "ZahlKond", String.Empty)

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.KSTNr = SafeGetInteger(reader, "KSTNr", 0)
						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.RPLNr = SafeGetInteger(reader, "RPLNr", 0)
						data.LANr = SafeGetDecimal(reader, "LANr", 0)

						data.K_Anzahl = SafeGetDecimal(reader, "K_Anzahl", 0)
						data.K_Ansatz = SafeGetDecimal(reader, "K_Ansatz", 0)
						data.K_Basis = SafeGetDecimal(reader, "K_Basis", 0)
						data.K_Betrag = SafeGetDecimal(reader, "K_Betrag", 0)
						data.MwSt = SafeGetDecimal(reader, "MwSt", 0)

						data.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
						data.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)

						data.KstBez = SafeGetString(reader, "KstBez", String.Empty)
						data.RPZusatztext = SafeGetString(reader, "RPZusatztext", String.Empty)
						data.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0", String.Empty)

						data.GAV_FAG = SafeGetDecimal(reader, "GAV_FAG", 0)
						data.GAV_VAG = SafeGetDecimal(reader, "GAV_VAG", 0)
						data.GAV_WAG = SafeGetDecimal(reader, "GAV_WAG", 0)

						data.MAName = SafeGetString(reader, "MAName", String.Empty)
						data.OPVersand = SafeGetString(reader, "OPVersand", String.Empty)
						data.KDFirma1 = SafeGetString(reader, "KDFirma1", String.Empty)
						data.KDFirma2 = SafeGetString(reader, "KDFirma2", String.Empty)
						data.KDFirma3 = SafeGetString(reader, "KDFirma3", String.Empty)
						data.KDPostfach = SafeGetString(reader, "KDPostfach", String.Empty)
						data.KDStrasse = SafeGetString(reader, "KDStrasse", String.Empty)
						data.KDPLZ = SafeGetString(reader, "KDPLZ", String.Empty)
						data.KDOrt = SafeGetString(reader, "KDOrt", String.Empty)
						data.KDLand = SafeGetString(reader, "KDLand", String.Empty)
						data.AnzKopien = SafeGetInteger(reader, "AnzKopien", 1)

						data.KDMwStNr = SafeGetString(reader, "KDMwStNr", String.Empty)
						data.Send2WOS = SafeGetBoolean(reader, "Send2WOS", False)
						data.LAOPText = SafeGetString(reader, "LAOPText", String.Empty)

						data.RPText = SafeGetString(reader, "RPText", String.Empty)
						data.KST_Ort_PLZ = SafeGetString(reader, "KST_Ort_PLZ", String.Empty)
						data.KST_PK_PLZ = SafeGetString(reader, "KST_PK_PLZ", String.Empty)
						data.KST_Res_Info_1 = SafeGetString(reader, "KST_Res_Info_1", String.Empty)
						data.KST_Res_Info_2 = SafeGetString(reader, "KST_Res_Info_2", String.Empty)
						data.DTAKontoNr = SafeGetString(reader, "DTAKontoNr", String.Empty)
						data.IBANDTA = SafeGetString(reader, "IBANDTA", String.Empty)
						data.ESRBankName = SafeGetString(reader, "ESRBankName", String.Empty)
						data.ESRBankAdresse = SafeGetString(reader, "ESRBankAdresse", String.Empty)
						data.ESR_Swift = SafeGetString(reader, "ESR_Swift", String.Empty)
						data.ESR_IBAN1 = SafeGetString(reader, "ESR_IBAN1", String.Empty)
						data.ESR_IBAN2 = SafeGetString(reader, "ESR_IBAN2", String.Empty)
						data.ESR_IBAN3 = SafeGetString(reader, "ESR_IBAN3", String.Empty)

						data.ESR_BCNr = SafeGetString(reader, "ESR_BCNr", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)
						data.MDEMail = SafeGetString(reader, "MDEMail", String.Empty)
						data.MDHomepage = SafeGetString(reader, "MDHomepage", String.Empty)
						data.Bemerk_RE = SafeGetString(reader, "Bemerk_RE", String.Empty)
						data.Bemerk_LO = SafeGetString(reader, "Bemerk_LO", String.Empty)
						data.Bemerk_P = SafeGetString(reader, "Bemerk_P", String.Empty)
						data.Bemerk_1 = SafeGetString(reader, "Bemerk_1", String.Empty)
						data.Bemerk_2 = SafeGetString(reader, "Bemerk_2", String.Empty)
						data.Bemerk_3 = SafeGetString(reader, "Bemerk_3", String.Empty)
						data.Ende = SafeGetString(reader, "Ende", String.Empty)
						data.KDZHDData = SafeGetString(reader, "KDZHDData", String.Empty)
						data.DismissalOn = SafeGetDateTime(reader, "DismissalOn", Nothing)
						data.DismissalFor = SafeGetDateTime(reader, "DismissalFor", Nothing)
						data.DismissalKind = SafeGetString(reader, "DismissalKind", String.Empty)
						data.DismissalWho = SafeGetString(reader, "dismissalwho", String.Empty)
						data.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", 0)
						data.RPVon = SafeGetDateTime(reader, "RPVon", Nothing)
						data.RPBis = SafeGetDateTime(reader, "RPBis", Nothing)


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

		Function LoadAssignedCustomInvoicePrintData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer, ByVal customerLanguage As String) As IEnumerable(Of InvoicePrintData) Implements IListingDatabaseAccess.LoadAssignedCustomInvoicePrintData

			Dim result As List(Of InvoicePrintData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "[Get Custom InvoiceData For Print]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerLanguage", ReplaceMissing(customerLanguage, "D")))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoicePrintData)

					While reader.Read
						Dim data = New InvoicePrintData

						data.Art = SafeGetString(reader, "Art", String.Empty)
						data.RENr = SafeGetInteger(reader, "RENr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)

						data.KST = SafeGetString(reader, "KST", String.Empty)
						data.Fak_Dat = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						data.ValutaData = SafeGetDateTime(reader, "ValutaData", Nothing)
						data.Currency = SafeGetString(reader, "Currency", String.Empty)

						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.MwSt1 = SafeGetDecimal(reader, "MwSt1", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)

						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)

						data.R_Name1 = SafeGetString(reader, "R_Name1", String.Empty)
						data.R_Name2 = SafeGetString(reader, "R_Name2", String.Empty)
						data.R_Name3 = SafeGetString(reader, "R_Name3", String.Empty)
						data.R_ZHD = SafeGetString(reader, "R_ZHD", String.Empty)
						data.R_Postfach = SafeGetString(reader, "R_Postfach", String.Empty)
						data.R_Strasse = SafeGetString(reader, "R_Strasse", String.Empty)
						data.R_Abteilung = SafeGetString(reader, "R_Abteilung", String.Empty)

						data.MwStProz = SafeGetDecimal(reader, "MwStProz", 8)

						data.PLZOrt = SafeGetString(reader, "PLZOrt", String.Empty)
						data.R_Land = SafeGetString(reader, "R_Land", String.Empty)
						data.RefNr = SafeGetString(reader, "RefNr", String.Empty)
						data.RefFootNr = SafeGetString(reader, "RefFootNr", String.Empty)
						data.ESRID = SafeGetString(reader, "ESRID", String.Empty)
						data.ESRKonto = SafeGetString(reader, "ESRKonto", String.Empty)
						data.KontoNr = SafeGetString(reader, "KontoNr", String.Empty)
						data.btrFr = SafeGetInteger(reader, "btrFr", 0)
						data.BtrRP = SafeGetInteger(reader, "BtrRP", 0)
						data.ZahlKond = SafeGetString(reader, "ZahlKond", String.Empty)

						data.IndMWST = SafeGetDecimal(reader, "IndMWST", 0)
						data.IndBetragEx = SafeGetDecimal(reader, "IndBetragEx", 0)
						data.BetragTotal = SafeGetDecimal(reader, "BetragTotal", 0)
						data.RE_HeadText = SafeGetString(reader, "RE_HeadText")
						data.RE_Text = SafeGetString(reader, "RE_Text")

						data.OPVersand = SafeGetString(reader, "OPVersand", String.Empty)
						data.KDFirma1 = SafeGetString(reader, "KDFirma1", String.Empty)
						data.KDFirma2 = SafeGetString(reader, "KDFirma2", String.Empty)
						data.KDFirma3 = SafeGetString(reader, "KDFirma3", String.Empty)
						data.KDPostfach = SafeGetString(reader, "KDPostfach", String.Empty)
						data.KDStrasse = SafeGetString(reader, "KDStrasse", String.Empty)
						data.KDOrt = SafeGetString(reader, "KDOrt", String.Empty)
						data.AnzKopien = SafeGetInteger(reader, "AnzKopien", 1)

						data.KDMwStNr = SafeGetString(reader, "KDMwStNr", String.Empty)
						data.Send2WOS = SafeGetBoolean(reader, "Send2WOS", False)

						data.DTAKontoNr = SafeGetString(reader, "DTAKontoNr", String.Empty)
						data.IBANDTA = SafeGetString(reader, "IBANDTA", String.Empty)
						data.ESRBankName = SafeGetString(reader, "ESRBankName", String.Empty)
						data.ESRBankAdresse = SafeGetString(reader, "ESRBankAdresse", String.Empty)
						data.ESR_Swift = SafeGetString(reader, "ESR_Swift", String.Empty)
						data.ESR_IBAN1 = SafeGetString(reader, "ESR_IBAN1", String.Empty)
						data.ESR_IBAN2 = SafeGetString(reader, "ESR_IBAN2", String.Empty)
						data.ESR_IBAN3 = SafeGetString(reader, "ESR_IBAN3", String.Empty)

						data.ESR_BCNr = SafeGetString(reader, "ESR_BCNr", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)
						data.MDEMail = SafeGetString(reader, "MDEMail", String.Empty)
						data.MDHomepage = SafeGetString(reader, "MDHomepage", String.Empty)
						data.KDZHDData = SafeGetString(reader, "KDZHDData", String.Empty)


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

		Function LoadAssignedCreditInvoicePrintData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer, ByVal customerLanguage As String) As IEnumerable(Of InvoicePrintData) Implements IListingDatabaseAccess.LoadAssignedCreditInvoicePrintData

			Dim result As List(Of InvoicePrintData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "[Get Credit InvoiceData For Print]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerLanguage", ReplaceMissing(customerLanguage, "D")))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoicePrintData)

					While reader.Read
						Dim data = New InvoicePrintData

						data.Art = SafeGetString(reader, "Art", String.Empty)
						data.RENr = SafeGetInteger(reader, "RENr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)

						data.KST = SafeGetString(reader, "KST", String.Empty)
						data.Fak_Dat = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						data.Currency = SafeGetString(reader, "Currency", String.Empty)

						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.MwSt1 = SafeGetDecimal(reader, "MwSt1", 0)

						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)

						data.R_Name1 = SafeGetString(reader, "R_Name1", String.Empty)
						data.R_Name2 = SafeGetString(reader, "R_Name2", String.Empty)
						data.R_Name3 = SafeGetString(reader, "R_Name3", String.Empty)
						data.R_ZHD = SafeGetString(reader, "R_ZHD", String.Empty)
						data.R_Postfach = SafeGetString(reader, "R_Postfach", String.Empty)
						data.R_Strasse = SafeGetString(reader, "R_Strasse", String.Empty)
						data.R_Abteilung = SafeGetString(reader, "R_Abteilung", String.Empty)

						data.MwStProz = SafeGetDecimal(reader, "MwStProz", 8)

						data.PLZOrt = SafeGetString(reader, "PLZOrt", String.Empty)
						data.R_Land = SafeGetString(reader, "R_Land", String.Empty)
						data.RefNr = SafeGetString(reader, "RefNr", String.Empty)
						data.RefFootNr = SafeGetString(reader, "RefFootNr", String.Empty)
						data.ESRID = SafeGetString(reader, "ESRID", String.Empty)
						data.ESRKonto = SafeGetString(reader, "ESRKonto", String.Empty)
						data.KontoNr = SafeGetString(reader, "KontoNr", String.Empty)
						data.btrFr = SafeGetInteger(reader, "btrFr", 0)
						data.BtrRP = SafeGetInteger(reader, "BtrRP", 0)
						data.ZahlKond = SafeGetString(reader, "ZahlKond", String.Empty)

						data.IndMWST = SafeGetDecimal(reader, "IndMWST", 0)
						data.IndBetragEx = SafeGetDecimal(reader, "IndBetragEx", 0)
						data.BetragTotal = SafeGetDecimal(reader, "BetragTotal", 0)
						data.RE_HeadText = SafeGetString(reader, "RE_HeadText")
						data.RE_Text = SafeGetString(reader, "RE_Text")

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.KSTNr = SafeGetInteger(reader, "KSTNr", 0)
						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.RPLNr = SafeGetInteger(reader, "RPLNr", 0)
						data.LANr = SafeGetDecimal(reader, "LANr", 0)

						data.K_Anzahl = SafeGetDecimal(reader, "K_Anzahl", 0)
						data.K_Ansatz = SafeGetDecimal(reader, "K_Ansatz", 0)
						data.K_Basis = SafeGetDecimal(reader, "K_Basis", 0)
						data.K_Betrag = SafeGetDecimal(reader, "K_Betrag", 0)
						data.MwSt = SafeGetDecimal(reader, "MwSt", 0)

						data.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
						data.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)

						data.KstBez = SafeGetString(reader, "KstBez", String.Empty)

						data.MAName = SafeGetString(reader, "MAName", String.Empty)
						data.OPVersand = SafeGetString(reader, "OPVersand", String.Empty)
						data.KDFirma1 = SafeGetString(reader, "KDFirma1", String.Empty)
						data.KDFirma2 = SafeGetString(reader, "KDFirma2", String.Empty)
						data.KDFirma3 = SafeGetString(reader, "KDFirma3", String.Empty)
						data.KDPostfach = SafeGetString(reader, "KDPostfach", String.Empty)
						data.KDStrasse = SafeGetString(reader, "KDStrasse", String.Empty)
						data.KDOrt = SafeGetString(reader, "KDOrt", String.Empty)
						data.AnzKopien = SafeGetInteger(reader, "AnzKopien", 1)

						data.KDMwStNr = SafeGetString(reader, "KDMwStNr", String.Empty)
						data.Send2WOS = SafeGetBoolean(reader, "Send2WOS", False)
						data.LAOPText = SafeGetString(reader, "LAOPText", String.Empty)

						data.RPText = SafeGetString(reader, "RPText", String.Empty)
						data.DTAKontoNr = SafeGetString(reader, "DTAKontoNr", String.Empty)
						data.IBANDTA = SafeGetString(reader, "IBANDTA", String.Empty)
						data.ESRBankName = SafeGetString(reader, "ESRBankName", String.Empty)
						data.ESRBankAdresse = SafeGetString(reader, "ESRBankAdresse", String.Empty)
						data.ESR_Swift = SafeGetString(reader, "ESR_Swift", String.Empty)
						data.ESR_IBAN1 = SafeGetString(reader, "ESR_IBAN1", String.Empty)
						data.ESR_IBAN2 = SafeGetString(reader, "ESR_IBAN2", String.Empty)
						data.ESR_IBAN3 = SafeGetString(reader, "ESR_IBAN3", String.Empty)

						data.ESR_BCNr = SafeGetString(reader, "ESR_BCNr", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)
						data.MDEMail = SafeGetString(reader, "MDEMail", String.Empty)
						data.MDHomepage = SafeGetString(reader, "MDHomepage", String.Empty)
						data.Bemerk_RE = SafeGetString(reader, "Bemerk_RE", String.Empty)
						data.Bemerk_LO = SafeGetString(reader, "Bemerk_LO", String.Empty)
						data.Bemerk_P = SafeGetString(reader, "Bemerk_P", String.Empty)
						data.Bemerk_1 = SafeGetString(reader, "Bemerk_1", String.Empty)
						data.Bemerk_2 = SafeGetString(reader, "Bemerk_2", String.Empty)
						data.Bemerk_3 = SafeGetString(reader, "Bemerk_3", String.Empty)

						data.RPVon = SafeGetDateTime(reader, "RPVon", Nothing)
						data.RPBis = SafeGetDateTime(reader, "RPBis", Nothing)


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

		Function LoadAssignedRefundInvoicePrintData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer, ByVal customerLanguage As String) As IEnumerable(Of InvoicePrintData) Implements IListingDatabaseAccess.LoadAssignedRefundInvoicePrintData

			Dim result As List(Of InvoicePrintData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "[Get Refund InvoiceData For Print]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerLanguage", ReplaceMissing(customerLanguage, "D")))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoicePrintData)

					While reader.Read
						Dim data = New InvoicePrintData

						data.Art = SafeGetString(reader, "Art", String.Empty)
						data.RENr = SafeGetInteger(reader, "RENr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)

						data.KST = SafeGetString(reader, "KST", String.Empty)
						data.Fak_Dat = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						data.Currency = SafeGetString(reader, "Currency", String.Empty)

						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.MwSt1 = SafeGetDecimal(reader, "MwSt1", 0)

						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)

						data.R_Name1 = SafeGetString(reader, "R_Name1", String.Empty)
						data.R_Name2 = SafeGetString(reader, "R_Name2", String.Empty)
						data.R_Name3 = SafeGetString(reader, "R_Name3", String.Empty)
						data.R_ZHD = SafeGetString(reader, "R_ZHD", String.Empty)
						data.R_Postfach = SafeGetString(reader, "R_Postfach", String.Empty)
						data.R_Strasse = SafeGetString(reader, "R_Strasse", String.Empty)
						data.R_Abteilung = SafeGetString(reader, "R_Abteilung", String.Empty)

						data.MwStProz = SafeGetDecimal(reader, "MwStProz", 8)

						data.PLZOrt = SafeGetString(reader, "PLZOrt", String.Empty)
						data.R_Land = SafeGetString(reader, "R_Land", String.Empty)
						data.RefNr = SafeGetString(reader, "RefNr", String.Empty)
						data.RefFootNr = SafeGetString(reader, "RefFootNr", String.Empty)
						data.ESRID = SafeGetString(reader, "ESRID", String.Empty)
						data.ESRKonto = SafeGetString(reader, "ESRKonto", String.Empty)
						data.KontoNr = SafeGetString(reader, "KontoNr", String.Empty)
						data.btrFr = SafeGetInteger(reader, "btrFr", 0)
						data.BtrRP = SafeGetInteger(reader, "BtrRP", 0)
						data.ZahlKond = SafeGetString(reader, "ZahlKond", String.Empty)

						data.IndMWST = SafeGetDecimal(reader, "IndMWST", 0)
						data.IndBetragEx = SafeGetDecimal(reader, "IndBetragEx", 0)
						data.BetragTotal = SafeGetDecimal(reader, "BetragTotal", 0)
						data.RE_HeadText = SafeGetString(reader, "RE_HeadText")
						data.RE_Text = SafeGetString(reader, "RE_Text")

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.KSTNr = SafeGetInteger(reader, "KSTNr", 0)
						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.RPLNr = SafeGetInteger(reader, "RPLNr", 0)
						data.LANr = SafeGetDecimal(reader, "LANr", 0)

						data.K_Anzahl = SafeGetDecimal(reader, "K_Anzahl", 0)
						data.K_Ansatz = SafeGetDecimal(reader, "K_Ansatz", 0)
						data.K_Basis = SafeGetDecimal(reader, "K_Basis", 0)
						data.K_Betrag = SafeGetDecimal(reader, "K_Betrag", 0)
						data.MwSt = SafeGetDecimal(reader, "MwSt", 0)

						data.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
						data.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)

						data.KstBez = SafeGetString(reader, "KstBez", String.Empty)

						data.MAName = SafeGetString(reader, "MAName", String.Empty)
						data.OPVersand = SafeGetString(reader, "OPVersand", String.Empty)
						data.KDFirma1 = SafeGetString(reader, "KDFirma1", String.Empty)
						data.KDFirma2 = SafeGetString(reader, "KDFirma2", String.Empty)
						data.KDFirma3 = SafeGetString(reader, "KDFirma3", String.Empty)
						data.KDPostfach = SafeGetString(reader, "KDPostfach", String.Empty)
						data.KDStrasse = SafeGetString(reader, "KDStrasse", String.Empty)
						data.KDOrt = SafeGetString(reader, "KDOrt", String.Empty)
						data.AnzKopien = SafeGetInteger(reader, "AnzKopien", 1)

						data.KDMwStNr = SafeGetString(reader, "KDMwStNr", String.Empty)
						data.Send2WOS = SafeGetBoolean(reader, "Send2WOS", False)
						data.LAOPText = SafeGetString(reader, "LAOPText", String.Empty)

						data.RPText = SafeGetString(reader, "RPText", String.Empty)
						data.DTAKontoNr = SafeGetString(reader, "DTAKontoNr", String.Empty)
						data.IBANDTA = SafeGetString(reader, "IBANDTA", String.Empty)
						data.ESRBankName = SafeGetString(reader, "ESRBankName", String.Empty)
						data.ESRBankAdresse = SafeGetString(reader, "ESRBankAdresse", String.Empty)
						data.ESR_Swift = SafeGetString(reader, "ESR_Swift", String.Empty)
						data.ESR_IBAN1 = SafeGetString(reader, "ESR_IBAN1", String.Empty)
						data.ESR_IBAN2 = SafeGetString(reader, "ESR_IBAN2", String.Empty)
						data.ESR_IBAN3 = SafeGetString(reader, "ESR_IBAN3", String.Empty)

						data.ESR_BCNr = SafeGetString(reader, "ESR_BCNr", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)
						data.MDEMail = SafeGetString(reader, "MDEMail", String.Empty)
						data.MDHomepage = SafeGetString(reader, "MDHomepage", String.Empty)
						data.Bemerk_RE = SafeGetString(reader, "Bemerk_RE", String.Empty)
						data.Bemerk_LO = SafeGetString(reader, "Bemerk_LO", String.Empty)
						data.Bemerk_P = SafeGetString(reader, "Bemerk_P", String.Empty)
						data.Bemerk_1 = SafeGetString(reader, "Bemerk_1", String.Empty)
						data.Bemerk_2 = SafeGetString(reader, "Bemerk_2", String.Empty)
						data.Bemerk_3 = SafeGetString(reader, "Bemerk_3", String.Empty)

						data.RPVon = SafeGetDateTime(reader, "RPVon", Nothing)
						data.RPBis = SafeGetDateTime(reader, "RPBis", Nothing)


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

		Function LoadInvoiceReportData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of InvoiceReportData) Implements IListingDatabaseAccess.LoadInvoiceReportData

			Dim result As List(Of InvoiceReportData) = Nothing

			Dim sql As String

			sql = "Select RPL.RPNr"
			sql &= ",(Select Top 1 Bezeichnung From KD_KST Where KDNr = RPL.KDNr And RecNr = RPL.KSTNr) As KSTBez"
			sql &= ",(Select Top 1 LA.LAOpText From LA Where LAJahr = Year(GetDate()) And LA.LANr = RPL.LANr And LA.LADeactivated = 0) As LABez"
			sql &= ",Sum(RPL.K_Anzahl) As Anzahl"
			sql &= ",Sum(RPL.K_Betrag) As Betrag"

			sql &= " From RPL"
			sql &= " Left Join RP On RPL.RPNr = RP.RPNr"
			sql &= " Where RPL.RENr = @RENumber"
			sql &= " Group by RPL.RPNr, RPL.KSTNr, RPL.LANr, RPL.KDNr"
			sql &= " Order by RPL.RPNr ASC, RPL.LANr ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RENumber", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoiceReportData)

					While reader.Read
						Dim data = New InvoiceReportData

						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.KSTBez = SafeGetString(reader, "KSTBez")
						data.LABez = SafeGetString(reader, "LABez")
						data.Anzahl = SafeGetDecimal(reader, "Anzahl", 0)
						data.Betrag = SafeGetDecimal(reader, "Betrag", 0)


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

		Function LoadInvoiceReportLinesGroupedByKSTData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of InvoiceReportData) Implements IListingDatabaseAccess.LoadInvoiceReportLinesGroupedByKSTData

			Dim result As List(Of InvoiceReportData) = Nothing

			Dim sql As String

			sql = "Select "
			sql &= "(Select Top 1 Bezeichnung From KD_KST Where KDNr = RPL.KDNr And RecNr = RPL.KSTNr) As KSTBez"
			sql &= ",(Select Top 1 LA.LAOpText From LA Where LAJahr = Year(GetDate()) And LA.LANr = RPL.LANr And LA.LADeactivated = 0) As LABez"
			sql &= ",Sum(RPL.K_Anzahl) As Anzahl"
			sql &= ",Sum(RPL.K_Betrag) As Betrag"

			sql &= " From RPL"
			sql &= " Where RPL.RENr = @RENumber"
			sql &= " Group by RPL.KSTNr, RPL.LANr, RPL.KDNr"
			sql &= " Order by RPL.KSTNr ASC, RPL.LANr ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RENumber", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoiceReportData)

					While reader.Read
						Dim data = New InvoiceReportData

						data.KSTBez = SafeGetString(reader, "KSTBez")
						data.LABez = SafeGetString(reader, "LABez")
						data.Anzahl = SafeGetDecimal(reader, "Anzahl", 0)
						data.Betrag = SafeGetDecimal(reader, "Betrag", 0)


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

		Function LoadInvoiceReportLinesGroupedData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of InvoiceReportData) Implements IListingDatabaseAccess.LoadInvoiceReportLinesGroupedData

			Dim result As List(Of InvoiceReportData) = Nothing

			Dim sql As String

			sql = "Select "
			sql &= "(Select Top 1 LA.LAOpText From LA Where LAJahr = Year(GetDate()) And LA.LANr = RPL.LANr And LA.LADeactivated = 0) As LABez"
			sql &= ",Sum(RPL.K_Anzahl) As Anzahl"
			sql &= ",Sum(RPL.K_Betrag) As Betrag"

			sql &= " From RPL"
			sql &= " Where RPL.RENr = @RENumber"
			sql &= " Group by RPL.LANr"
			sql &= " Order by RPL.LANr ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RENumber", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoiceReportData)

					While reader.Read
						Dim data = New InvoiceReportData

						data.LABez = SafeGetString(reader, "LABez")
						data.Anzahl = SafeGetDecimal(reader, "Anzahl", 0)
						data.Betrag = SafeGetDecimal(reader, "Betrag", 0)

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

		Function UpdateInvoiceDatabaseWithPrintDate(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As Boolean Implements IListingDatabaseAccess.UpdateInvoiceDatabaseWithPrintDate
			Dim success = True

			Dim sql As String

			sql = "Update RE Set PrintedDate = GetDate()"
			sql &= " Where RE.MDNr = @mdNr And RENr = @RENr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function


		Function LoadReportDataForPrintedInvoice(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of ReportForPrintInvoiceData) Implements IListingDatabaseAccess.LoadReportDataForPrintedInvoice

			Dim result As List(Of ReportForPrintInvoiceData) = Nothing

			Dim sql As String

			sql = "SELECT s.id"
			sql &= ",s.rpnr"
			sql &= ",s.Beschreibung"
			sql &= ",s.ChangedFrom"
			sql &= ",s.ChangedOn"
			sql &= ",s.ScanExtension"
			sql &= ",s.DocScan"
			sql &= ",s.RPDoc_Guid"
			sql &= ",RPL.RENr"
			sql &= ",RPL.RPLNr"
			sql &= ",RPL.ESNr"
			sql &= ",RPL.KDNr"
			sql &= ",RPL.MANr"

			sql &= " FROM dbo.RP_ScanDoc s "
			sql &= " Left Join RPL On s.RPLNr = RPL.RPLNr and s.RPNr = RPL.RPNr "
			sql &= " Where RPL.RENr = @RENr"
			sql &= " And s.DocScan Is Not Null "
			sql &= " Order By s.RecNr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportForPrintInvoiceData)

					While reader.Read
						Dim data = New ReportForPrintInvoiceData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.RENr = SafeGetInteger(reader, "RENr", 0)
						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.RPLNr = SafeGetInteger(reader, "RPLNr", 0)

						data.Beschreibung = SafeGetString(reader, "Beschreibung")
						data.DocScan = SafeGetByteArray(reader, "DocScan")
						data.Extension = SafeGetString(reader, "ScanExtension")
						data.RPDoc_Guid = SafeGetString(reader, "RPDoc_Guid")

						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")


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

