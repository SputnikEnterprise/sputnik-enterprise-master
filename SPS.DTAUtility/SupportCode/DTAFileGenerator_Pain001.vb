

Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Xml.Schema
Imports SP.DatabaseAccess.DTAUtility.DataObjects



Partial Class DTAFileGenerator

	Dim m_MDBankData As BankData

	Public Function GenerateHeaderXml() As XDocument

		If PaymentListData Is Nothing Then Return Nothing

		Dim mdData = m_DTAUtilityDatabaseAccess.LoadMandantAndBankDataForDTAFileCreation(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear, BankIDNumber)

		Try
			Dim xDoc As XDocument = MapDbVacanciesToXML()

			Return xDoc

		Catch ex As Exception
			Return Nothing

		End Try


	End Function

	Private Function MapDbVacanciesToXML() As XDocument

		' Map each db data to jobCh data structure.
		Dim xmlDoc = ToXDoc()

		If xmlDoc Is Nothing Then
			'Throw New Exception(String.Format("Convert to xml failed (CustomerGuid={0}). Errors={1}", m_CustomerGuid, vacancies.ValidationErrors))
		End If

		Return xmlDoc

	End Function

	Private Function ToXDoc() As XDocument

		Dim xDoc As New XDocument(New XDeclaration("1.0", "utf-8", "true"))
		xDoc.Add(LoadCstmrCdtTrfInitn())

		Return xDoc

	End Function

	Private Function LoadCstmrCdtTrfInitn() As XElement

		If PaymentListData Is Nothing Then Return Nothing

		Dim request As New XElement("Document")
		Dim result As New XElement("CstmrCdtTrfInitn")

		result.Add(LoadGrpHdr)
		result.Add(LoadPmtInf)

		request.Add(result)


		Return request

	End Function

	Private Function LoadGrpHdr() As XElement

		Dim amount As Decimal = PaymentListData.Sum(Function(item) Math.Abs(item.Betrag.Value))

		Dim GrpHdr = New XElement("GrpHdr")
		GrpHdr.Add(New XElement("MsgId", Guid.NewGuid.ToString().Replace("-", "")))
		GrpHdr.Add(New XElement("CreDtTm", Now))
		GrpHdr.Add(New XElement("NbOfTxs", PaymentListData.Count))
		GrpHdr.Add(New XElement("CtrlSum", String.Format("{0:f2}", amount)))

		Dim InitgPty = New XElement("InitgPty")
		InitgPty.Add(New XElement("Nm", m_InitializationData.MDData.MDGuid))

		Dim CtctDtls = New XElement("CtctDtls")
		CtctDtls.Add(New XElement("Nm", "Sputnik Enterprise Suite"))
		CtctDtls.Add(New XElement("Othr", "2.0.20"))

		InitgPty.Add(CtctDtls)
		GrpHdr.Add(InitgPty)


		Return GrpHdr

	End Function

	Private Function LoadPmtInf() As XElement

		Dim companyIBAN As String = m_MDData.IBANDTA.Trim.Replace(" ", "").Trim

		Dim PmtInf = New XElement("PmtInf")
		PmtInf.Add(New XElement("PmtInfId", Guid.NewGuid.ToString().Replace("-", "")))
		PmtInf.Add(New XElement("PmtMtd", "TRF"))
		PmtInf.Add(New XElement("BtchBookg", "true"))
		PmtInf.Add(New XElement("NbOfTxs", PaymentListData.Count))

		Dim PmtTpInf = New XElement("PmtTpInf")
		If m_MarkAsSalary Then
			Dim CtgyPurp = New XElement("CtgyPurp")
			CtgyPurp.Add(New XElement("Cd", "SALA"))

			PmtTpInf.Add(CtgyPurp)
			'Else
			'	CtgyPurp.Add(New XElement("Cd", "OTHR"))
		End If
		PmtInf.Add(PmtTpInf)

		PmtInf.Add(New XElement("ReqdExctnDt", String.Format("{0:yyyy-MM-dd}", PaymentDate)))

		Dim Dbtr = New XElement("Dbtr")
		Dbtr.Add(New XElement("Nm", m_MDData.DTA_Name))
		PmtInf.Add(Dbtr)

		Dim DbtrAcct = New XElement("DbtrAcct")
		Dim Id = New XElement("Id")
		Id.Add(New XElement("IBAN", companyIBAN))
		Dim Tp = New XElement("Tp")
		Tp.Add(New XElement("Prtry", "CWD")) '"CND"))
		DbtrAcct.Add(Id)
		DbtrAcct.Add(Tp)
		PmtInf.Add(DbtrAcct)

		Dim DbtrAgt = New XElement("DbtrAgt")
		Dim FinInstnId = New XElement("FinInstnId")
		FinInstnId.Add(New XElement("BIC", m_MDData.IID.ToString.Replace(" ", "").Trim))
		DbtrAgt.Add(FinInstnId)
		PmtInf.Add(DbtrAgt)

		For Each zg In PaymentListData
			Trace.WriteLine(zg.ZGNr)
			If Not ValidatePaymentCdtTrfTxInfData(zg) Then Return Nothing

			Dim data = LoadCdtTrfTxInf(zg)
			If data Is Nothing Then Return Nothing
			PmtInf.Add(data)
		Next


		Return PmtInf

	End Function

	Private Function LoadCdtTrfTxInf(ByVal zg As ZgData) As XElement

		Dim amount As Decimal = Math.Abs(zg.Betrag.GetValueOrDefault(0))
		Dim employeeFullname As String = zg.DTAAdr1.Trim
		If employeeFullname.Length > 0 Then employeeFullname = employeeFullname.Substring(0, Math.Min(employeeFullname.Length, 70))

		Dim employeeIBAN As String = zg.IBANNr.Replace(" ", "").Trim
		Dim employeeCountryAdrLine As String = If(employeeIBAN.Length > 20, employeeIBAN.Substring(0, Math.Min(employeeIBAN.Length, 2)), zg.EmployeeCountry)

		Dim employeeFirstAdrLine As String = zg.DTAAdr2.Trim.Replace("!", "").Replace("?", "")
		If employeeFirstAdrLine.Length > 0 Then employeeFirstAdrLine = employeeFirstAdrLine.Substring(0, Math.Min(employeeFirstAdrLine.Length, 35))
		Dim employeeSecondAdrLine As String = zg.DTAAdr3.Trim.Replace("!", "").Replace("?", "")
		If employeeSecondAdrLine.Length > 0 Then employeeSecondAdrLine = employeeSecondAdrLine.Substring(0, Math.Min(employeeSecondAdrLine.Length, 35))

		'Dim X As String = "([a-zA-Z0-9\.,;:'\+\-/\(\)?\*\[\]\{\}\\`´~ ]|[!#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*"
		'Dim Test1 = Regex.Replace(employeeFirstAdrLine, X, "")
		'Dim test2 = Regex.Replace(employeeSecondAdrLine, X, "")

		Dim paymentReason As String = zg.ZGGrund.Trim

		If paymentReason.Contains("Lohnpfändung") Then
			paymentReason = String.Format("Lohnpfändung: {0}", employeeFirstAdrLine)
		End If

		If paymentReason.Length > 0 Then
			paymentReason = paymentReason.Substring(0, Math.Min(paymentReason.Trim.Length, 140))
		Else
			paymentReason = "à Konto"
		End If

		Dim CdtTrfTxInf = New XElement("CdtTrfTxInf")
		Dim PmtId As XElement = <PmtId>
									<InstrId><%= Guid.NewGuid.ToString().Replace("-", "") %></InstrId>
									<EndToEndId><%= Guid.NewGuid.ToString().Replace("-", "") %></EndToEndId>
								</PmtId>

		CdtTrfTxInf.Add(PmtId)

		Dim Amt As XElement = <Amt>
								  <InstdAmt Ccy=<%= "CHF" %>><%= String.Format("{0:f2}", amount) %></InstdAmt>
							  </Amt>
		CdtTrfTxInf.Add(Amt)
		If ChrgBrENum = Charger.DEBT Then
			CdtTrfTxInf.Add(New XElement("ChrgBr", "DEBT"))
		ElseIf ChrgBrENum = Charger.CRED Then
			CdtTrfTxInf.Add(New XElement("ChrgBr", "CRED"))
		Else
			CdtTrfTxInf.Add(New XElement("ChrgBr", "SHAR"))
		End If

		Dim CdtrAgt As XElement = <CdtrAgt>
									  <FinInstnId>
										  <BIC><%= zg.Swift.ToString.Replace(" ", "").Trim %></BIC>
									  </FinInstnId>
								  </CdtrAgt>
		CdtTrfTxInf.Add(CdtrAgt)

		Dim Cdtr = New XElement("Cdtr")
		Cdtr.Add(New XElement("Nm", employeeFullname))
		Dim PstlAdr = New XElement("PstlAdr")
		If Not String.IsNullOrWhiteSpace(employeeCountryAdrLine) Then PstlAdr.Add(New XElement("Ctry", employeeCountryAdrLine))
		PstlAdr.Add(New XElement("AdrLine", employeeFirstAdrLine))
		If Not String.IsNullOrWhiteSpace(employeeSecondAdrLine) Then PstlAdr.Add(New XElement("AdrLine", employeeSecondAdrLine))
		Cdtr.Add(PstlAdr)
		CdtTrfTxInf.Add(Cdtr)


		Dim CdtrAcct As XElement
		If employeeIBAN.Length < 15 Then

			CdtrAcct = <CdtrAcct>
						   <Id>
							   <Othr>
								   <Id><%= employeeIBAN %></Id>
							   </Othr>
						   </Id>
					   </CdtrAcct>

			Dim UltmtCdtr As XElement
			Dim bankLocationData As New List(Of String)
			Dim streetName As String
			bankLocationData = zg.BankOrt.Split(",").ToList()
			streetName = bankLocationData(0)
			Dim townName As String = If(bankLocationData.Count > 1, bankLocationData(1), bankLocationData(0))
			Dim countryName As String = bankLocationData(bankLocationData.Count - 1)
			If townName = countryName Then countryName = employeeCountryAdrLine

			UltmtCdtr = <UltmtCdtr>
							<Nm><%= zg.Bank %></Nm>
							<PstlAdr>
								<StrtNm><%= streetName %></StrtNm>
								<TwnNm><%= townName %></TwnNm>
								<Ctry><%= countryName %></Ctry>
							</PstlAdr>
						</UltmtCdtr>

			CdtTrfTxInf.Add(CdtrAcct)
			If Not (String.IsNullOrWhiteSpace(streetName) AndAlso String.IsNullOrWhiteSpace(townName) AndAlso String.IsNullOrWhiteSpace(countryName)) Then CdtTrfTxInf.Add(UltmtCdtr)

		Else
			CdtrAcct = <CdtrAcct>
						   <Id>
							   <IBAN><%= employeeIBAN %></IBAN>
						   </Id>
					   </CdtrAcct>

			CdtTrfTxInf.Add(CdtrAcct)
		End If

		Dim RmtInf As XElement
		RmtInf = <RmtInf>
					 <Ustrd><%= paymentReason %></Ustrd>
				 </RmtInf>
		CdtTrfTxInf.Add(RmtInf)


		Return CdtTrfTxInf

	End Function

	Private Function ValidatePaymentCdtTrfTxInfData(ByVal zg As ZgData) As Boolean
		Dim result As Boolean = True
		Dim msg As String = String.Empty

		Dim swiftData As String = zg.Swift.ToString.Replace(" ", "").Trim
		If String.IsNullOrWhiteSpace(swiftData) Then
			msg = "Fehlende SWIFT-Daten!{0}Kandidatennummer: {1}{0}{0}Ihre Datei kann nicht erstellt werden."
			msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine, zg.MANr)
			result = False
		End If

		Dim employeeIBAN As String = zg.IBANNr.Replace(" ", "").Trim
		Dim employeeCountryAdrLine As String = If(employeeIBAN.Length > 20, employeeIBAN.Substring(0, Math.Min(employeeIBAN.Length, 2)), zg.EmployeeCountry)

		If String.IsNullOrWhiteSpace(employeeCountryAdrLine) Then
			msg = "Fehlende IBAN bzw. Land-Daten!{0}Kandidatennummer: {1}{0}{0}Ihre Datei kann nicht erstellt werden."
			msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine, zg.MANr)

			result = False
		End If


		Dim employeeFirstAdrLine As String = zg.DTAAdr2.Trim
		If employeeFirstAdrLine.Length > 0 Then employeeFirstAdrLine = employeeFirstAdrLine.Substring(0, Math.Min(employeeFirstAdrLine.Length, 35))
		Dim employeeSecondAdrLine As String = zg.DTAAdr3.Trim
		If employeeSecondAdrLine.Length > 0 Then employeeSecondAdrLine = employeeSecondAdrLine.Substring(0, Math.Min(employeeSecondAdrLine.Length, 35))

		If String.IsNullOrWhiteSpace(employeeFirstAdrLine) OrElse String.IsNullOrWhiteSpace(employeeSecondAdrLine) Then
			msg = "Fehlende Adress-Daten!{0}Kandidatennummer: {1}{0}{0}Ihre Datei kann nicht erstellt werden."
			msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine, zg.MANr)

			result = False
		End If

		If Not result Then
			m_UtilityUI.ShowInfoDialog(msg, m_Translate.GetSafeTranslationValue("Pain.001 erstellen"), MessageBoxIcon.Warning)
			Throw New Exception(msg)
		End If

		Return result

	End Function

End Class
