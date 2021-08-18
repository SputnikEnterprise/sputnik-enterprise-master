Imports SP.DatabaseAccess.DTAUtility.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports System.IO
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.ESRUtility.DataObjects

Public Class EsrFileParser


#Region "private consts"

	Private Const CAMT054_TXDTLS As String = "TxDtls"


#End Region

#Region "Private Fields"

	''' <summary>
	''' The DAT utility data access object.
	''' </summary>
	Private m_esrDbAccess As SP.DatabaseAccess.ESRUtility.IESRUtilityDatabaseAccess

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

	Private m_SelectedFile As String
	Private m_NS As XNamespace


#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal esrDbAccess As SP.DatabaseAccess.ESRUtility.IESRUtilityDatabaseAccess, ByVal init As SP.Infrastructure.Initialization.InitializeClass)

		m_esrDbAccess = esrDbAccess

		m_InitializationData = init
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

	End Sub

#End Region


#Region "public properties"

	Public Property BankNumber As Integer
	Public Property Use7Digit As Boolean

#End Region


#Region "Public Methods"

#Region "camt.054"

	Public Function ParseCamt054File(ByVal filePath As String) As ESRRecord_Camt054
		' Datei prüfen
		If Not System.IO.File.Exists(filePath) Then
			Return Nothing
		End If

		' Datei zeilenweise einlesen und ESR Liste füllen
		m_SelectedFile = filePath

		Dim xelement As XElement = XElement.Load(m_SelectedFile)
		m_NS = xelement.Name.Namespace
		Dim txdtls = LoadTransactionDetails(xelement)


		Return txdtls

	End Function


#End Region


#Region "esr modul"

	Public Function ParseEsrFile(ByVal filePath As String) As EsrFileData
		' Datei prüfen
		If Not System.IO.File.Exists(filePath) Then
			Return Nothing
		End If

		' Datei zeilenweise einlesen und ESR Liste füllen
		Dim esrDataList = New EsrFileData()
		Try
			Using fileReader = System.IO.File.OpenText(filePath)
				Dim line As String
				Do
					line = fileReader.ReadLine()
					If line IsNot Nothing Then
						esrDataList.Add(line, Use7Digit)
					Else
						Exit Do
					End If
				Loop
			End Using
		Catch ex As Exception
			Return Nothing
		End Try

		' Rückgabe bei Erfolg
		Return esrDataList
	End Function

#End Region


#End Region


	Private Function LoadTransactionDetails(ByVal xEL As XElement) As ESRRecord_Camt054
		Dim result As ESRRecord_Camt054 = Nothing

		Dim camt0054Data = New ESRRecord_Camt054

		Dim BkToCstmrDbtCdtNtfctn = From TransactionDetails In xEL.Elements(m_NS + "BkToCstmrDbtCdtNtfctn")
																Select TransactionDetails
		For Each header In BkToCstmrDbtCdtNtfctn
			Dim grpHdr As New GrpHdrData
			grpHdr = LoadHeader(header)
			If grpHdr Is Nothing Then Return Nothing

			Dim Ntfctn As New NtfctnData
			Ntfctn = LoadNtfctn(header)

			camt0054Data.GrpHdr = grpHdr
			camt0054Data.Ntfctn = Ntfctn
		Next
		If camt0054Data Is Nothing Then Return camt0054Data


		Try
			result = camt0054Data


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

			Return result
		End Try

		Return result

	End Function

	Private Function LoadHeader(ByVal xEL As XElement) As GrpHdrData
		Dim result As GrpHdrData = Nothing

		Dim personalinfomationData = New GrpHdrData

		Try
			Dim GrpHdrView = From TransactionDetails In xEL.Elements(m_NS + "GrpHdr")
											 Select TransactionDetails

			For Each XEL1 As XElement In GrpHdrView

				personalinfomationData.MsgId = GetSafeStringFromXElement(XEL1.Element(m_NS + "MsgId"))
				personalinfomationData.CreDtTm = GetSafeDateFromXElement(XEL1.Element(m_NS + "CreDtTm"))

				Dim MsgRcptView = From TransactionDetails In XEL1.Elements(m_NS + "MsgRcpt")
													Select TransactionDetails
				If Not MsgRcptView Is Nothing Then
					If Not XEL1.Element(m_NS + "MsgRcpt") Is Nothing Then
						personalinfomationData.MsgRcpt = GetSafeStringFromXElement(XEL1.Element(m_NS + "MsgRcpt").Element(m_NS + "Nm"))
					End If
				End If

				personalinfomationData.AddtlInf = GetSafeStringFromXElement(XEL1.Element(m_NS + "AddtlInf"))

			Next
			result = personalinfomationData


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

			Return result
		End Try

		Return result

	End Function

	Private Function LoadNtfctn(ByVal xEL As XElement) As NtfctnData
		Dim result As NtfctnData = Nothing

		Dim personalinfomationData = New NtfctnData

		Try
			Dim NtfctnView = From TransactionDetails In xEL.Elements(m_NS + "Ntfctn")
											 Select TransactionDetails

			For Each XEL1 As XElement In NtfctnView

				personalinfomationData.Id = GetSafeStringFromXElement(XEL1.Element(m_NS + "Id"))
				personalinfomationData.ElctrncSeqNb = GetSafeIntegerFromXElement(XEL1.Element(m_NS + "ElctrncSeqNb"))

				personalinfomationData.CreDtTm = GetSafeDateFromXElement(XEL1.Element(m_NS + "CreDtTm"))
				'Dim dateFromTo = GetSafeDateFromXElement(XEL1.Element(m_NS + "FrToDt"))
				If Not XEL1.Element(m_NS + "FrToDt") Is Nothing Then
					personalinfomationData.FrDtTm = GetSafeDateFromXElement(XEL1.Element(m_NS + "FrToDt").Element(m_NS + "FrDtTm"))
					personalinfomationData.ToDtTm = GetSafeDateFromXElement(XEL1.Element(m_NS + "FrToDt").Element(m_NS + "ToDtTm"))
				End If
				personalinfomationData.CpyDplctInd = GetSafeStringFromXElement(XEL1.Element(m_NS + "CpyDplctInd"))

				Dim Acct = New AcctData
				Acct = LoadAcctData(XEL1)
				personalinfomationData.Acct = Acct


				Dim NtryElement = From TransactionDetails In XEL1.Elements(m_NS + "Ntry")
													Select TransactionDetails
				Dim ntryListData As New List(Of NtryData)
				For Each Ntry As XElement In NtryElement
					Dim ntryView As New NtryData

					ntryView.NtryRef = GetSafeStringFromXElement(Ntry.Element(m_NS + "NtryRef"))
					ntryView.Amt = GetSafeDoubleFromXElement(Ntry.Element(m_NS + "Amt"))
					ntryView.CdtDbtInd = GetSafeStringFromXElement(Ntry.Element(m_NS + "CdtDbtInd"))
					ntryView.RvslInd = GetSafeBooleanFromXElement(Ntry.Element(m_NS + "RvslInd"))
					ntryView.Sts = GetSafeStringFromXElement(Ntry.Element(m_NS + "Sts"))
					ntryView.BookgDt = GetSafeDateFromXElement(Ntry.Element(m_NS + "BookgDt").Element(m_NS + "Dt"))
					ntryView.ValDt = GetSafeDateFromXElement(Ntry.Element(m_NS + "ValDt").Element(m_NS + "Dt"))
					ntryView.AcctSvcrRef = GetSafeStringFromXElement(Ntry.Element(m_NS + "AcctSvcrRef"))

					Dim NtryDtlsElement = From TransactionDetails In Ntry.Elements(m_NS + "NtryDtls")
																Select TransactionDetails
					Dim NtryDtlsListData As New NtryDtlsData
					For Each NtryDtl As XElement In NtryDtlsElement
						Dim ntryDtlView As New NtryDtlsData

						'Dim btch = GetSafeDoubleFromXElement(NtryDtl.Element(m_NS + "Btch"))
						Dim btch = GetSafeStringFromXElement(NtryDtl.Element(m_NS + "Btch"))
						If Not btch Is Nothing AndAlso Not String.IsNullOrWhiteSpace(btch) Then ntryDtlView.Btch = GetSafeIntegerFromXElement(NtryDtl.Element(m_NS + "Btch").Element(m_NS + "NbOfTxs"))

						Dim TxDtls As New List(Of TxDtlsData)
						TxDtls = LoadTxDtlsData(NtryDtl)

						ntryDtlView.TxDtls = TxDtls
						NtryDtlsListData.TxDtls = TxDtls
					Next

					ntryView.NtryDtls = NtryDtlsListData
					ntryView.AddtlNtryInf = GetSafeStringFromXElement(Ntry.Element(m_NS + "AddtlNtryInf"))

					ntryListData.Add(ntryView)
				Next

				personalinfomationData.Ntry = ntryListData
			Next
			result = personalinfomationData


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

			Return result
		End Try

		Return result

	End Function

	Private Function LoadAcctData(ByVal xEL As XElement) As AcctData
		Dim result As AcctData = Nothing

		Dim personalinfomationData = New AcctData

		Try
			Dim NtfctnView = From TransactionDetails In xEL.Elements(m_NS + "Acct")
											 Select TransactionDetails

			For Each XEL1 As XElement In NtfctnView

				personalinfomationData.IBAN = GetSafeStringFromXElement(XEL1.Element(m_NS + "Id").Element(m_NS + "IBAN"))

				If Not XEL1.Element(m_NS + "Svcr") Is Nothing Then
					If Not GetSafeStringFromXElement(XEL1.Element(m_NS + "Svcr").Element(m_NS + "FinInstnId")) Is Nothing Then
						If Not XEL1.Element(m_NS + "Svcr").Element(m_NS + "FinInstnId") Is Nothing Then
							personalinfomationData.Nm = GetSafeStringFromXElement(XEL1.Element(m_NS + "Svcr").Element(m_NS + "FinInstnId").Element(m_NS + "Nm"))
						End If
					End If
				End If

				If Not XEL1.Element(m_NS + "Ownr") Is Nothing Then
					personalinfomationData.OwnerNm = GetSafeStringFromXElement(XEL1.Element(m_NS + "Ownr").Element(m_NS + "Nm"))
					If Not XEL1.Element(m_NS + "Ownr").Element(m_NS + "PstlAdr") Is Nothing Then
						personalinfomationData.OwnerAdrLine = GetSafeStringFromXElement(XEL1.Element(m_NS + "Ownr").Element(m_NS + "PstlAdr").Element(m_NS + "AdrLine"))
					End If
				End If

			Next
			result = personalinfomationData


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

			Return result
		End Try

		Return result

	End Function

	Private Function LoadTxDtlsData(ByVal xEL As XElement) As List(Of TxDtlsData)
		Dim result As List(Of TxDtlsData) = Nothing

		Dim personalinfomationData As New List(Of TxDtlsData)

		Try
			Dim TxDtlsView = From TransactionDetails In xEL.Elements(m_NS + "TxDtls")
											 Select TransactionDetails

			For Each XEL1 As XElement In TxDtlsView
				Dim txDtlView As New TxDtlsData

				Dim refsView = From TransactionDetails In XEL1.Elements(m_NS + "Refs")
											 Select TransactionDetails
				Dim refs As New TxDtlsRef
				For Each ref In refsView
					refs.AcctSvcrRef = GetSafeStringFromXElement(ref.Element(m_NS + "AcctSvcrRef"))
					refs.EndToEndId = GetSafeStringFromXElement(ref.Element(m_NS + "EndToEndId"))
					If Not ref.Element(m_NS + "Prtry") Is Nothing Then
						refs.Tp = GetSafeStringFromXElement(ref.Element(m_NS + "Prtry").Element(m_NS + "Tp"))
						refs.Ref = GetSafeStringFromXElement(ref.Element(m_NS + "Prtry").Element(m_NS + "Ref"))
					End If
				Next
				txDtlView.Refs = refs

				If Not XEL1.Element(m_NS + "AmtDtls") Is Nothing Then
					If Not XEL1.Element(m_NS + "AmtDtls").Element(m_NS + "InstdAmt") Is Nothing Then
						txDtlView.Amount = GetSafeDoubleFromXElement(XEL1.Element(m_NS + "AmtDtls").Element(m_NS + "InstdAmt").Element(m_NS + "Amt"))
					Else
						txDtlView.Amount = GetSafeDoubleFromXElement(XEL1.Element(m_NS + "Amt"))
					End If
				Else
					txDtlView.Amount = GetSafeDoubleFromXElement(XEL1.Element(m_NS + "Amt"))

				End If
				txDtlView.CdtDbtInd = GetSafeStringFromXElement(XEL1.Element(m_NS + "CdtDbtInd"))
				If Not XEL1.Element(m_NS + "RltdPties") Is Nothing Then
					If Not XEL1.Element(m_NS + "RltdPties").Element(m_NS + "Dbtr") Is Nothing Then
						txDtlView.RltdPtiesNm = GetSafeStringFromXElement(XEL1.Element(m_NS + "RltdPties").Element(m_NS + "Dbtr").Element(m_NS + "Nm"))
					End If
				End If


				Dim RmtInfView = From TransactionDetails In XEL1.Elements(m_NS + "RmtInf").Elements(m_NS + "Strd").Elements(m_NS + "CdtrRefInf")
												 Select TransactionDetails
				Dim rmtInf As New TxDtlsRmtInf
				For Each itm In RmtInfView
					If Not itm.Element(m_NS + "Tp") Is Nothing Then
						If Not itm.Element(m_NS + "Tp").Element(m_NS + "CdOrPrtry") Is Nothing Then
							rmtInf.Prtry = GetSafeStringFromXElement(itm.Element(m_NS + "Tp").Element(m_NS + "CdOrPrtry").Element(m_NS + "Prtry"))
						End If
					End If
					rmtInf.Ref = GetSafeStringFromXElement(itm.Element(m_NS + "Ref"))

					rmtInf.InvoiceNumber = LoadInvoiceNumber(rmtInf.Ref, Use7Digit)
					rmtInf.CustomerNumber = CustomerNumber(rmtInf.Ref, Use7Digit)

				Next

				txDtlView.RmtInf = rmtInf

				If Not XEL1.Element(m_NS + "RltdDts") Is Nothing Then
					txDtlView.AccptncDtTm = GetSafeDateFromXElement(XEL1.Element(m_NS + "RltdDts").Element(m_NS + "AccptncDtTm"))
				End If


				personalinfomationData.Add(txDtlView)
			Next

			result = personalinfomationData


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

			Return result
		End Try

		Return result

	End Function


#Region "Helpers"


	Private Function LoadInvoiceNumber(ByVal refData As String, ByVal use7Digit As Boolean) As Integer?


		Dim value As Integer = 0D
		If use7Digit Then
			If refData.Length >= 25 Then
				Decimal.TryParse(refData.Substring(17, 7), value)
			End If
		Else
			If refData.Length >= 25 Then
				Decimal.TryParse(refData.Substring(16, 10), value)
			End If
		End If

		Return value

	End Function

	Private Function CustomerNumber(ByVal refData As String, ByVal use7Digit As Boolean) As Integer?

		Dim value As Integer = 0D
		If use7Digit Then
			If refData.Length >= 25 Then
				Decimal.TryParse(refData.Substring(7, 7), value)
			End If
		Else
			If refData.Length >= 25 Then
				Decimal.TryParse(refData.Substring(6, 10), value)
			End If
		End If

		Return value

	End Function

	Private Function GetSafeStringFromXElement(ByVal xelment As XElement) As String

		If xelment Is Nothing Then
			Return String.Empty
		Else

			Return xelment.Value
		End If

	End Function

	Private Function GetSafeDateFromXElement(ByVal xelment As XElement) As Date?

		If xelment Is Nothing Then
			Return Nothing
		Else

			If xelment.Value.ToString.Contains("T24:00:00") Then
				Return CDate(xelment.Value.ToString.Replace("T24:00:00", "T23:59:59"))
			End If
			Return CDate(xelment.Value)
		End If

	End Function

	Private Function GetSafeBooleanFromXElement(ByVal xelment As XElement) As Boolean?

		If xelment Is Nothing Then
			Return Nothing
		Else

			Return CBool(xelment.Value)
		End If

	End Function

	Private Function GetSafeIntegerFromXElement(ByVal xelment As XElement) As Integer?

		If xelment Is Nothing Then
			Return Nothing
		Else

			Return CInt(Val(xelment.Value))
		End If

	End Function

	Private Function GetSafeDoubleFromXElement(ByVal xelment As XElement) As Double?

		If xelment Is Nothing Then
			Return Nothing
		Else

			Return CDbl(Val(xelment.Value))
		End If

	End Function

	Private Function GetSafeByteFromXElement(ByVal xelment As XElement) As Byte()

		If xelment Is Nothing Then
			Return Nothing
		Else

			'Dim utf8 As Encoding = Encoding.UTF8()
			Dim bytes As Byte() = Convert.FromBase64String(xelment.Value) ' utf8.GetBytes(xelment.Value)


			Return (bytes)
		End If

	End Function

#End Region


End Class
