Imports SP.DatabaseAccess.DTAUtility.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports System.IO
Imports SP.Infrastructure.Logging
Imports System.Text

Public Class DTAFileGenerator

#Region "Private Fields"

  ''' <summary>
  ''' The DAT utility data access object.
  ''' </summary>
  Private m_DTAUtilityDatabaseAccess As SP.DatabaseAccess.DTAUtility.IDTAUtilityDatabaseAccess

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_md As Mandant

  ''' <summary>
  ''' The logger.
  ''' </summary>
  Private Shared m_Logger As ILogger = New Logger()

  ''' <summary>
  ''' UI Utility functions.
  ''' </summary>
  Private m_UtilityUI As New UtilityUI

	Private DTATotalAmount As Decimal?
	Private DTATotalrecordcount As Integer?
	Public m_CreatedPaymentFile As String

	Private m_MDData As MandantAndBankDataForDTAFileCreation
	Private m_MarkAsSalary As Boolean


#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal dtaDBAccess As SP.DatabaseAccess.DTAUtility.IDTAUtilityDatabaseAccess, ByVal m_init As SP.Infrastructure.Initialization.InitializeClass)

		m_DTAUtilityDatabaseAccess = dtaDBAccess
		m_InitializationData = m_init

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

	End Sub

#End Region


#Region "public properties"


	Public Property PaymentListData As IEnumerable(Of ZgData)
	Public Property PaymentDate As Date
	Public Property BankIDNumber As Integer
	Public Property ChrgBrENum As Charger

	Public Enum Charger
		DEBT  ' ex OUR
		CRED  ' ex BEN
		SHAR  ' ex. SHA
	End Enum

	Public ReadOnly Property DTAAmountValue() As Decimal?
		Get
			Return DTATotalAmount
		End Get
	End Property

	Public ReadOnly Property DTARecordCount() As Integer?
		Get
			Return DTATotalrecordcount
		End Get
	End Property

	Public ReadOnly Property CreatedPaymentFile
		Get
			Return m_CreatedPaymentFile
		End Get
	End Property


#End Region


#Region "Public Methods"

	Public Function BuildXMLPain001FileSwiss(ByVal mdYear As Integer, ByVal markAsSalary As Boolean, ByVal filePath As String) As Integer

		' Zahlung in Datenbank aktualisieren
		Dim newDtaNr As Integer? = m_DTAUtilityDatabaseAccess.GetNewDtaNr(0)
		Dim zgDataList = PaymentListData
		Dim dtaDate = PaymentDate
		Dim mdNr = m_InitializationData.MDData.MDNr
		Dim bankIDNumber As Integer = Me.BankIDNumber

		If newDtaNr.GetValueOrDefault(0) = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Neue Nummer für DTA-Auftrag konnte nicht ermittelt werden. Der Vorgang wird abgebrochen."))

			Return 0
		End If

		If zgDataList Is Nothing OrElse zgDataList.Count = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Zahlung wurde ausgewählt."))

			Return 0
		End If

		m_MarkAsSalary = markAsSalary
		m_MDData = m_DTAUtilityDatabaseAccess.LoadMandantAndBankDataForDTAFileCreation(mdNr, mdYear, bankIDNumber)
		If String.IsNullOrWhiteSpace(m_MDData.IBANDTA) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Bankverbindung enthält keine IBAN-Nummer. Bitte tragen Sie die fehlende Daten ein."))

			Return 0
		End If

		Try
			Dim xDoc = GenerateHeaderXml()

			Dim xmlns As XNamespace = "http://www.six-interbank-clearing.com/de/pain.001.001.03.ch.02.xsd"
			Dim xsi As XNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance")
			Dim blank As XNamespace = "http://www.six-interbank-clearing.com/de/pain.001.001.03.ch.02.xsd"

			If xDoc Is Nothing Then Return 0

			Dim result = SaveXmlDocument(xDoc, filePath)
			If Not File.Exists(result) Then Return 0

			Dim lines() As String = System.IO.File.ReadAllLines(result)
			If lines(1).StartsWith("<Document>") Then
				lines(1) = "<Document xmlns=" + Chr(34) + xmlns.ToString + Chr(34) + " xmlns:xsi=" + Chr(34) + xsi.ToString + Chr(34) + ">"
				System.IO.File.WriteAllLines(result, lines)
			End If

			If File.Exists(result) Then
				For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
					m_DTAUtilityDatabaseAccess.UpdateZGrecForDTAFile(zgData.ZGNr, newDtaNr, dtaDate)
				Next
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return 0
		End Try

		Return newDtaNr

	End Function

	Public Function BuildXMLPain001FileForeign(ByVal mdYear As Integer, ByVal markAsSalary As Boolean, ByVal filePath As String) As Integer

		' Zahlung in Datenbank aktualisieren
		Dim newDtaNr As Integer? = m_DTAUtilityDatabaseAccess.GetNewDtaNr(0)
		Dim zgDataList = PaymentListData
		Dim dtaDate = PaymentDate
		Dim mdNr = m_InitializationData.MDData.MDNr
		Dim bankRecordNumber As Integer = BankIDNumber

		If newDtaNr.GetValueOrDefault(0) = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Neue Nummer für DTA-Auftrag konnte nicht ermittelt werden. Der Vorgang wird abgebrochen."))

			Return 0
		End If
		m_MarkAsSalary = markAsSalary
		m_MDData = m_DTAUtilityDatabaseAccess.LoadMandantAndBankDataForDTAFileCreation(mdNr, mdYear, bankRecordNumber)

		Try
			Dim xDoc = GenerateHeaderXml()

			Dim xmlns As XNamespace = "http://www.six-interbank-clearing.com/de/pain.001.001.03.ch.02.xsd"
			Dim xsi As XNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance")
			Dim blank As XNamespace = "http://www.six-interbank-clearing.com/de/pain.001.001.03.ch.02.xsd"

			If xDoc Is Nothing Then Return 0

			Dim result = SaveXmlDocument(xDoc, filePath)
			If Not File.Exists(result) Then Return 0

			Dim lines() As String = System.IO.File.ReadAllLines(result)
			If lines(1).StartsWith("<Document>") Then
				lines(1) = "<Document xmlns=" + Chr(34) + xmlns.ToString + Chr(34) + " xmlns:xsi=" + Chr(34) + xsi.ToString + Chr(34) + ">"
				System.IO.File.WriteAllLines(result, lines)
			End If

			If File.Exists(result) Then
				For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
					m_DTAUtilityDatabaseAccess.UpdateZGrecForDTAFile(zgData.ZGNr, newDtaNr, dtaDate)
				Next
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("error in pain.001 modul: {0}", ex.ToString))
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Fehler in Pain.001 Modul.{0},{1}"), vbNewLine, ex.ToString))

			Return 0
		End Try

		Return newDtaNr

	End Function

	Public Function BuildXMLPain001FileCreditorSwiss(ByVal mdYear As Integer, ByVal markAsSalary As Boolean, ByVal filePath As String) As Integer

		' Zahlung in Datenbank aktualisieren
		Dim newDtaNr As Integer? = m_DTAUtilityDatabaseAccess.GetNewDtaNr(0)
		Dim zgDataList = PaymentListData
		Dim dtaDate = PaymentDate
		Dim mdNr = m_InitializationData.MDData.MDNr
		Dim bankRecordNumber As Integer = BankIDNumber

		If newDtaNr.GetValueOrDefault(0) = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Neue Nummer für DTA-Auftrag konnte nicht ermittelt werden. Der Vorgang wird abgebrochen."))

			Return 0
		End If

		m_MDData = m_DTAUtilityDatabaseAccess.LoadMandantAndBankDataForDTAFileCreation(mdNr, mdYear, bankRecordNumber)

		Try
			Dim xDoc = GenerateHeaderXml()

			Dim xmlns As XNamespace = "http://www.six-interbank-clearing.com/de/pain.001.001.03.ch.02.xsd"
			Dim xsi As XNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance")
			Dim blank As XNamespace = "http://www.six-interbank-clearing.com/de/pain.001.001.03.ch.02.xsd"

			If xDoc Is Nothing Then Return 0

			Dim result = SaveXmlDocument(xDoc, filePath)
			If Not File.Exists(result) Then Return 0

			Dim lines() As String = System.IO.File.ReadAllLines(result)
			If lines(1).StartsWith("<Document>") Then
				lines(1) = "<Document xmlns=" + Chr(34) + xmlns.ToString + Chr(34) + " xmlns:xsi=" + Chr(34) + xsi.ToString + Chr(34) + ">"
				System.IO.File.WriteAllLines(result, lines)
			End If

			If File.Exists(result) Then
				For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
					m_DTAUtilityDatabaseAccess.UpdateLOLrecForDTAFile(zgData.ZGNr, newDtaNr, dtaDate)
				Next
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("error in pain.001 modul: {0}", ex.ToString))
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Fehler in Pain.001 Modul.{0},{1}"), vbNewLine, ex.ToString))

			Return 0
		End Try

		Return newDtaNr

	End Function



	'Try
	'		m_MDData = m_DTAUtilityDatabaseAccess.LoadMandantAndBankDataForDTAFileCreation(mdNr, mdYear, bankRecordNumber)

	'		Dim dtaDateShortString As String = dtaDate.ToString("yyMMdd")
	'		Dim todayShortString As String = dtaDateShortString

	'		' Mandant Kontonummer
	'		Dim mdLedgerNo As String
	'		If String.IsNullOrEmpty(m_MDData.IBANDTA) Then
	'			mdLedgerNo = m_MDData.DTA_Konto
	'		Else
	'			mdLedgerNo = m_MDData.IBANDTA.Replace(" ", "")
	'		End If

	'		Dim recordNo As Integer = 1
	'		Dim tactNo As Integer = 1
	'		Dim amountTotal As Decimal = 0D
	'		Dim sb As New StringBuilder()
	'		For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
	'			Dim amount = Math.Abs(zgData.Betrag.Value)
	'			If amount > 0D Then
	'				' Zahlung Clearingnummer
	'				Dim clearingNo As Integer = zgData.ClearingNr.GetValueOrDefault(0)
	'				If clearingNo = 99999 Then clearingNo = 9000

	'				Dim clearingNoString As String = If(clearingNo > 0, clearingNo.ToString(), String.Empty)

	'				' Zahlung Kontonummer
	'				Dim zgLedgerNo As String
	'				If String.IsNullOrEmpty(zgData.IBANNr) Then
	'					zgLedgerNo = zgData.KontoNr
	'				Else
	'					zgLedgerNo = zgData.IBANNr.Replace(" ", "")
	'				End If

	'				' Freitext
	'				Dim infoText As String = "ZG: " & zgData.ZGNr.GetValueOrDefault(0).ToString() & " (" & UCase(zgData.DTAAdr1) & " / " & UCase(zgData.DTAAdr1) & ")" & " / " & UCase(zgData.ZGGrund)

	'				'Dim infoText As String = "ZG: " & zgData.ZGNr.GetValueOrDefault(0).ToString() _
	'				'	 & " (" & UCase(zgData.DTAAdr1) & " / " & UCase(zgData.Nachname) & ", " & UCase(zgData.Vorname) & ")" _
	'				'	 & " / " & UCase(zgData.ZGGrund)

	'				' Record 01: TA 827
	'				sb.Append("01")
	'				sb.Append(dtaDateShortString)
	'				sb.Append(LSet(clearingNoString, 12))
	'				sb.Append("00000")
	'				sb.Append(todayShortString)
	'				sb.Append(LSet(m_MDData.DTA_BCNr, 7))
	'				sb.Append(LSet(m_MDData.DTA_ID, 5))
	'				sb.Append(recordNo.ToString("D5"))
	'				sb.Append("827")
	'				sb.Append(IIf(markAsSalary, "1", "0"))
	'				sb.Append("0")
	'				sb.Append(PadRight(m_MDData.DTA_ID, 5, " "))
	'				sb.Append(tactNo.ToString("D11"))
	'				sb.Append(LSet(mdLedgerNo, 24))
	'				sb.Append(Space(6))
	'				sb.Append("CHF")
	'				sb.Append(RSet(amount.ToString("0.00").Replace(".", ","), 12))
	'				sb.Append(Space(14))

	'				' Record 02
	'				sb.Append("02")
	'				sb.Append(LSet(UCase(m_MDData.DTA_Name), 24))
	'				sb.Append(LSet(UCase(m_MDData.DTA_Postfach), 24))
	'				sb.Append(LSet(UCase(m_MDData.DTA_Strasse), 24))
	'				sb.Append(LSet(UCase(m_MDData.DTA_PLZOrt), 24))
	'				sb.Append(Space(30))

	'				' Record 03
	'				sb.Append("03/C/")
	'				sb.Append(LSet(zgLedgerNo, 27))
	'				sb.Append(LSet(UCase(zgData.DTAAdr1), 24))
	'				sb.Append(LSet(UCase(zgData.DTAAdr2), 24))
	'				sb.Append(LSet(UCase(zgData.DTAAdr3), 24))
	'				sb.Append(LSet(UCase(zgData.DTAAdr4), 24))

	'				' Record 04
	'				sb.Append("04")
	'				sb.Append(LSet(infoText, 112))
	'				sb.Append(Space(14))

	'				recordNo += 1
	'				tactNo += 1
	'				amountTotal += amount
	'			End If
	'		Next

	'		If amountTotal > 0D Then
	'			' Total Record
	'			sb.Append("01000000")
	'			sb.Append(Space(12))
	'			sb.Append("00000")
	'			sb.Append(dtaDateShortString)
	'			sb.Append(Space(7))
	'			sb.Append(LSet(m_MDData.DTA_ID, 5))
	'			sb.Append(recordNo.ToString("D5"))
	'			sb.Append("89000")
	'			sb.Append(RSet(amountTotal.ToString("0.00").Replace(".", ","), 16))
	'			sb.Append(Space(59))

	'			' DTA Datei schreiben
	'			WriteStringToFile(filePath, sb.ToString())

	'			For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
	'				m_DTAUtilityDatabaseAccess.UpdateZGrecForDTAFile(zgData.ZGNr, newDtaNr, dtaDate)
	'			Next

	'		End If
	'		DTATotalAmount = amountTotal
	'		DTATotalrecordcount = recordNo


	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString())
	'		newDtaNr = 0

	'	End Try

	'	Return newDtaNr

	'End Function

	Protected Function SaveXmlDocument(ByVal xDoc As XDocument, ByVal fileName As String) As String

		Dim organsiationRootPath = System.IO.Path.Combine(m_InitializationData.UserData.spAllowedPath, m_InitializationData.MDData.MDGuid)
		If Directory.Exists(organsiationRootPath) Then Directory.CreateDirectory(organsiationRootPath)
		Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, String.Format("pain001_{0}.xml", DateTime.Now.Ticks))

		Try
			If File.Exists(fileName) Then File.Delete(fileName)
			If Path.GetExtension(fileName) <> ".xml" Then
				fileName = Path.ChangeExtension(fileName, ".XML")
			End If
			xmlFileName = fileName

		Catch ex As Exception
			fileName = xmlFileName
		End Try


		Try
			If File.Exists(xmlFileName) Then File.Delete(xmlFileName)
		Catch ex As Exception
			Threading.Thread.Sleep(500)
			File.Delete(xmlFileName)
		End Try

		Dim xmlCode As String = String.Empty

		Using sw As StringWriter = New StringWriter()
			xDoc.Save(sw)
			xmlCode = sw.ToString()
		End Using


		Try
			xDoc.Save(xmlFileName)
		Catch ex As Exception
			' Maybe the file is currently in use -> wait a little bit.
			Threading.Thread.Sleep(500)
			xDoc.Save(xmlFileName)
		End Try

		m_CreatedPaymentFile = xmlFileName


		Return xmlFileName
	End Function


	Public Function BuildNewDtaFileSwiss(
		ByVal zgDataList As IList(Of SP.DatabaseAccess.DTAUtility.DataObjects.ZgData),
		ByVal dtaDate As DateTime,
		ByVal mdNr As Integer,
		ByVal mdYear As Integer,
		ByVal bankIDNumber As Integer,
		ByVal markAsSalary As Boolean,
		ByVal filePath As String
		) As Integer

		' Zahlung in Datenbank aktualisieren
		Dim newDtaNr As Integer? = m_DTAUtilityDatabaseAccess.GetNewDtaNr(0)
		If newDtaNr.GetValueOrDefault(0) = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Neue Nummer für DTA-Auftrag konnte nicht ermittelt werden. Der Vorgang wird abgebrochen."))

			Return 0
		End If

		Try
			Dim mdData = m_DTAUtilityDatabaseAccess.LoadMandantAndBankDataForDTAFileCreation(mdNr, mdYear, bankIDNumber)

			Dim dtaDateShortString As String = dtaDate.ToString("yyMMdd")
			Dim todayShortString As String = dtaDateShortString

			' Mandant Kontonummer
			Dim mdLedgerNo As String
			If String.IsNullOrEmpty(mdData.IBANDTA) Then
				mdLedgerNo = mdData.DTA_Konto
			Else
				mdLedgerNo = mdData.IBANDTA.Replace(" ", "")
			End If

			Dim recordNo As Integer = 1
			Dim tactNo As Integer = 1
			Dim amountTotal As Decimal = 0D
			Dim sb As New StringBuilder()
			For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
				Dim amount = Math.Abs(zgData.Betrag.Value)
				If amount > 0D Then
					' Zahlung Clearingnummer
					Dim clearingNo As String = zgData.ClearingNr
					If Val(clearingNo) = 99999 Then clearingNo = "9000"

					Dim clearingNoString As String = If(Val(clearingNo) > 0, clearingNo.ToString(), String.Empty)

					' Zahlung Kontonummer
					Dim zgLedgerNo As String
					If String.IsNullOrEmpty(zgData.IBANNr) Then
						zgLedgerNo = zgData.KontoNr
					Else
						zgLedgerNo = zgData.IBANNr.Replace(" ", "")
					End If

					' Freitext
					Dim infoText As String = "ZG: " & zgData.ZGNr.GetValueOrDefault(0).ToString() & " (" & UCase(zgData.DTAAdr1) & " / " & UCase(zgData.DTAAdr1) & ")" & " / " & UCase(zgData.ZGGrund)

					'Dim infoText As String = "ZG: " & zgData.ZGNr.GetValueOrDefault(0).ToString() _
					'	 & " (" & UCase(zgData.DTAAdr1) & " / " & UCase(zgData.Nachname) & ", " & UCase(zgData.Vorname) & ")" _
					'	 & " / " & UCase(zgData.ZGGrund)

					' Record 01: TA 827
					sb.Append("01")
					sb.Append(dtaDateShortString)
					sb.Append(LSet(clearingNoString, 12))
					sb.Append("00000")
					sb.Append(todayShortString)
					sb.Append(LSet(mdData.DTA_BCNr, 7))
					sb.Append(LSet(mdData.DTA_ID, 5))
					sb.Append(recordNo.ToString("D5"))
					sb.Append("827")
					sb.Append(IIf(markAsSalary, "1", "0"))
					sb.Append("0")
					sb.Append(PadRight(mdData.DTA_ID, 5, " "))
					sb.Append(tactNo.ToString("D11"))
					sb.Append(LSet(mdLedgerNo, 24))
					sb.Append(Space(6))
					sb.Append("CHF")
					sb.Append(RSet(amount.ToString("0.00").Replace(".", ","), 12))
					sb.Append(Space(14))

					' Record 02
					sb.Append("02")
					sb.Append(LSet(UCase(mdData.DTA_Name), 24))
					sb.Append(LSet(UCase(mdData.DTA_Postfach), 24))
					sb.Append(LSet(UCase(mdData.DTA_Strasse), 24))
					sb.Append(LSet(UCase(mdData.DTA_PLZOrt), 24))
					sb.Append(Space(30))

					' Record 03
					sb.Append("03/C/")
					sb.Append(LSet(zgLedgerNo, 27))
					sb.Append(LSet(UCase(zgData.DTAAdr1), 24))
					sb.Append(LSet(UCase(zgData.DTAAdr2), 24))
					sb.Append(LSet(UCase(zgData.DTAAdr3), 24))
					sb.Append(LSet(UCase(zgData.DTAAdr4), 24))

					' Record 04
					sb.Append("04")
					sb.Append(LSet(infoText, 112))
					sb.Append(Space(14))

					recordNo += 1
					tactNo += 1
					amountTotal += amount
				End If
			Next

			If amountTotal > 0D Then
				' Total Record
				sb.Append("01000000")
				sb.Append(Space(12))
				sb.Append("00000")
				sb.Append(dtaDateShortString)
				sb.Append(Space(7))
				sb.Append(LSet(mdData.DTA_ID, 5))
				sb.Append(recordNo.ToString("D5"))
				sb.Append("89000")
				sb.Append(RSet(amountTotal.ToString("0.00").Replace(".", ","), 16))
				sb.Append(Space(59))

				' DTA Datei schreiben
				WriteStringToFile(filePath, sb.ToString())
				m_CreatedPaymentFile = filePath

				For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
					m_DTAUtilityDatabaseAccess.UpdateZGrecForDTAFile(zgData.ZGNr, newDtaNr, dtaDate)
				Next

			End If
			DTATotalAmount = amountTotal
			DTATotalrecordcount = recordNo


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			newDtaNr = 0

		End Try

		Return newDtaNr

	End Function

	Public Function BuildNewDtaFileForeign(
		ByVal zgDataList As IList(Of SP.DatabaseAccess.DTAUtility.DataObjects.ZgData),
		ByVal dtaDate As DateTime,
		ByVal mdNr As Integer,
		ByVal mdYear As Integer,
		ByVal bankIDNumber As Integer,
		ByVal markAsSalary As Boolean,
		ByVal bankChargesId As Integer,
		ByVal filePath As String
		) As Integer

		' Zahlung in Datenbank aktualisieren
		Dim newDtaNr As Integer = m_DTAUtilityDatabaseAccess.GetNewDtaNr(0)

		Try
			Dim mdData = m_DTAUtilityDatabaseAccess.LoadMandantAndBankDataForDTAFileCreation(mdNr, mdYear, bankIDNumber)

			Dim dtaDateShortString As String = dtaDate.ToString("yyMMdd")
			Dim todayShortString As String = Date.Today.ToString("yyMMdd")

			' Mandant Kontonummer
			Dim mdLedgerNo As String
			If String.IsNullOrEmpty(mdData.IBANDTA) Then
				mdLedgerNo = mdData.DTA_Konto
			Else
				mdLedgerNo = mdData.IBANDTA.Replace(" ", "")
			End If

			Dim recordNo As Integer = 1
			Dim tactNo As Integer = 1
			Dim amountTotal As Decimal = 0D
			Dim sb As New StringBuilder()
			For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
				Dim amount = Math.Abs(zgData.Betrag.Value)
				If amount > 0D Then
					' Bankdaten
					Dim bankDataCode As String
					Dim bankData01 As String
					Dim bankData02 As String
					If Not String.IsNullOrEmpty(zgData.Swift) Then
						bankDataCode = "A"
						bankData01 = zgData.Swift.Substring(0, Math.Min(zgData.Swift.Length, 11))
						bankData02 = String.Empty
					Else
						bankDataCode = "D"
						bankData01 = zgData.Bank
						bankData02 = zgData.BankOrt
					End If

					' Record 01: TA 836
					sb.Append("01")
					sb.Append("000000")
					sb.Append(Space(12))
					sb.Append("00000")
					sb.Append(todayShortString)
					sb.Append(LSet(mdData.DTA_BCNr, 7))
					sb.Append(LSet(mdData.DTA_ID, 5))
					sb.Append(recordNo.ToString("D5"))
					sb.Append("836")
					sb.Append(IIf(markAsSalary, "1", "0"))
					sb.Append("0")
					sb.Append(PadRight(mdData.DTA_ID, 5, " "))
					sb.Append(tactNo.ToString("D11"))
					sb.Append(LSet(mdLedgerNo, 24))
					sb.Append(dtaDateShortString)
					sb.Append(LSet(zgData.Currency, 3))
					' TODO: Fardin: Im VB6 ist der Betrag links statt rechts ausgerichtet
					sb.Append(RSet(amount.ToString("0.00").Replace(".", ","), 15))
					sb.Append(Space(11))

					' Record 02
					sb.Append("02")
					' TODO: Fardin: Devisenkurs (in VB6 auch nicht gesetzt)
					sb.Append(Space(12))
					sb.Append(LSet(UCase(mdData.DTA_Name), 35))
					sb.Append(LSet(UCase(mdData.DTA_Strasse), 35))
					sb.Append(LSet(UCase(mdData.DTA_PLZOrt), 35))
					sb.Append(Space(9))

					' Record 03: Adresse der Bank
					sb.Append("03")
					sb.Append(bankDataCode)
					sb.Append(LSet(bankData01, 35))
					sb.Append(LSet(bankData02, 35))
					sb.Append(LSet(zgData.IBANNr.Replace(" ", ""), 34))
					sb.Append(Space(21))

					' Record 04: Adresse des Begünstigten
					sb.Append("04")
					sb.Append(LSet(UCase(zgData.DTAAdr1), 35))
					sb.Append(LSet(UCase(zgData.DTAAdr2), 35))
					sb.Append(LSet(UCase(zgData.DTAAdr3), 35))
					sb.Append(Space(21))

					' Record 05
					sb.Append("05U")
					sb.Append(LSet("ZG: " & zgData.ZGNr.GetValueOrDefault(0).ToString() & " / " & UCase(zgData.ZGGrund), 35))
					sb.Append(LSet(UCase(zgData.DTAAdr1), 35))
					sb.Append(LSet(" (" & UCase(zgData.Nachname) & ", " & UCase(zgData.Nachname) & ")", 35))
					sb.Append(bankChargesId.ToString("D1"))
					sb.Append(Space(19))

					'' Zahlung in Datenbank aktualisieren
					'Dim newDtaNr As Integer? = m_DTAUtilityDatabaseAccess.GetNewDtaNr(0)
					'm_DTAUtilityDatabaseAccess.UpdateZGrecForDTAFile(zgData.ZGNr, newDtaNr, dtaDate)

					recordNo = recordNo + 1
					tactNo = tactNo + 1
					amountTotal += amount
				End If
			Next

			If amountTotal > 0D Then
				' Total Record
				sb.Append("01000000")
				sb.Append(Space(12))
				sb.Append("00000")
				sb.Append(dtaDateShortString)
				sb.Append(Space(7))
				sb.Append(LSet(mdData.DTA_ID, 5))
				sb.Append(recordNo.ToString("D5"))
				sb.Append("89000")
				sb.Append(RSet(amountTotal.ToString("0.00").Replace(".", ","), 16))
				sb.Append(Space(59))

				' DTA Datei schreiben
				WriteStringToFile(filePath, sb.ToString())
				m_CreatedPaymentFile = filePath

				For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
					m_DTAUtilityDatabaseAccess.UpdateZGrecForDTAFile(zgData.ZGNr, newDtaNr, dtaDate)
				Next

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			newDtaNr = 0

		End Try

		Return newDtaNr

	End Function

	Public Function BuildDtaFileCreditorSwiss(ByVal lolDataList As IList(Of SP.DatabaseAccess.DTAUtility.DataObjects.LolDataForDtaFileCreation),
																						ByVal dtaDate As DateTime, ByVal mdNr As Integer, ByVal mdYear As Integer, ByVal bankIDNumber As Integer, ByVal filePath As String) As Integer

		' Zahlung in Datenbank aktualisieren
		Dim newDtaNr As Integer = m_DTAUtilityDatabaseAccess.GetNewDtaNr(0)

		Try
			Dim mdData = m_DTAUtilityDatabaseAccess.LoadMandantAndBankDataForDTAFileCreation(mdNr, mdYear, bankIDNumber)

			Dim dtaDateShortString As String = dtaDate.ToString("yyMMdd")
			Dim todayShortString As String = dtaDateShortString

			' Mandant Kontonummer
			Dim mdLedgerNo As String
			If String.IsNullOrEmpty(mdData.IBANDTA) Then
				mdLedgerNo = mdData.DTA_Konto
			Else
				mdLedgerNo = mdData.IBANDTA.Replace(" ", "")
			End If

			Dim recordNo As Integer = 1
			Dim tactNo As Integer = 1
			Dim amountTotal As Decimal = 0D
			Dim sb As New StringBuilder()
			For Each lolData As SP.DatabaseAccess.DTAUtility.DataObjects.LolDataForDtaFileCreation In lolDataList
				Dim amount = Math.Abs(lolData.Betrag.Value)
				If amount > 0D Then
					' Zahlung Clearingnummer
					Dim clearingNo As String = lolData.DTABCNR
					If Val(clearingNo) = 99999 Then clearingNo = "9000"

					Dim clearingNoString As String = If(Val(clearingNo) > 0, clearingNo.ToString(), String.Empty)

					' Zahlung Kontonummer
					Dim zgLedgerNo As String
					If String.IsNullOrEmpty(lolData.IBANNr) Then
						zgLedgerNo = lolData.KONTONR
					Else
						zgLedgerNo = lolData.IBANNr.Replace(" ", "")
					End If

					' Freitext
					Dim infoText As String = "LO: " & lolData.LONr.GetValueOrDefault(0).ToString() _
						 & " (" & UCase(lolData.DTAADR1) & " / " & UCase(lolData.Nachname) & ", " & UCase(lolData.Vorname) & ")" _
						 & " / " & UCase(lolData.ZGGrund)

					' Record 01: TA 827
					sb.Append("01")
					sb.Append(dtaDateShortString)
					sb.Append(LSet(clearingNoString, 12))
					sb.Append("00000")
					sb.Append(todayShortString)
					sb.Append(LSet(mdData.DTA_BCNr, 7))
					sb.Append(LSet(mdData.DTA_ID, 5))
					sb.Append(recordNo.ToString("D5"))
					sb.Append("827")
					sb.Append("0")
					sb.Append("0")
					sb.Append(PadRight(mdData.DTA_ID, 5, " "))
					sb.Append(tactNo.ToString("D11"))
					sb.Append(LSet(mdLedgerNo, 24))
					sb.Append(Space(6))
					sb.Append("CHF")
					sb.Append(RSet(amount.ToString("0.00").Replace(".", ","), 12))
					sb.Append(Space(14))

					' Record 02
					sb.Append("02")
					sb.Append(LSet(UCase(mdData.DTA_Name), 24))
					sb.Append(LSet(UCase(mdData.DTA_Postfach), 24))
					sb.Append(LSet(UCase(mdData.DTA_Strasse), 24))
					sb.Append(LSet(UCase(mdData.DTA_PLZOrt), 24))
					sb.Append(Space(30))

					' Record 03
					sb.Append("03/C/")
					sb.Append(LSet(zgLedgerNo, 27))
					sb.Append(LSet(UCase(lolData.DTAADR1), 24))
					sb.Append(LSet(UCase(lolData.DTAADR2), 24))
					sb.Append(LSet(UCase(lolData.DTAADR3), 24))
					sb.Append(LSet(UCase(lolData.DTAADR4), 24))

					' Record 04
					sb.Append("04")
					sb.Append(LSet(infoText, 112))
					sb.Append(Space(14))

					recordNo += 1
					tactNo += 1
					amountTotal += amount
				End If
			Next

			If amountTotal > 0D Then
				' Total Record
				sb.Append("01000000")
				sb.Append(Space(12))
				sb.Append("00000")
				sb.Append(dtaDateShortString)
				sb.Append(Space(7))
				sb.Append(LSet(mdData.DTA_ID, 5))
				sb.Append(recordNo.ToString("D5"))
				sb.Append("89000")
				sb.Append(RSet(amountTotal.ToString("0.00").Replace(".", ","), 16))
				sb.Append(Space(59))

				' DTA Datei schreiben
				WriteStringToFile(filePath, sb.ToString())
				m_CreatedPaymentFile = filePath

				For Each lolData As SP.DatabaseAccess.DTAUtility.DataObjects.LolDataForDtaFileCreation In lolDataList
					m_DTAUtilityDatabaseAccess.UpdateLOLrecForDTAFile(lolData.LOLID, newDtaNr, dtaDate)
				Next

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			newDtaNr = 0

		End Try

		Return newDtaNr

	End Function

	Public Function BuildPaymentOrder(ByVal zgDataList As IList(Of SP.DatabaseAccess.DTAUtility.DataObjects.ZgData), ByVal dtaDate As DateTime) As Integer

		' Zahlung in Datenbank aktualisieren
		Dim newDtaNr As Integer? = m_DTAUtilityDatabaseAccess.GetNewDtaNr(0)
		Try
			Dim recordNo As Integer = 1
			Dim amountTotal As Decimal = 0D
			For Each zgData As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData In zgDataList
				Dim amount = Math.Abs(zgData.Betrag.Value)
				If amount > 0D Then

					m_DTAUtilityDatabaseAccess.UpdateZGrecForDTAFile(zgData.ZGNr, newDtaNr, dtaDate)

					recordNo += 1
					amountTotal += amount
				End If
			Next

			If amountTotal = 0D Then newDtaNr = 0


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			newDtaNr = 0

		End Try

		Return newDtaNr

	End Function


#End Region


#Region "Private Methods"

	Private Function PadRight(ByVal value As Object, _
                  ByVal fillCountAfter As Integer, _
                  ByVal fillString As String) As String

    value = Mid(value, 1, fillCountAfter)
    If fillCountAfter < Len(value) Then fillCountAfter = Len(value)
    Return value & New String(fillString, fillCountAfter - Len(value))

  End Function

  Private Sub WriteStringToFile(ByVal filename As String, ByVal text As String)

		File.WriteAllText(filename, text, System.Text.Encoding.Default)

  End Sub

#End Region

End Class
