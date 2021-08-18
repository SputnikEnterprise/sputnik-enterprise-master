

Imports SPS.ESRUtility.EsrRecord

Public Class ESRData

	Public Property amountDecision As PaymentProcessState
	Public Property EsrInvoiceNo As Integer?
	Public Property EsrCustomerNo As Integer?
	Public Property Customername As String
	Public Property EsrValutaDate As DateTime?
	Public Property EsrAmount As Double?
	Public Property InvoiceAmountOpen As Double?
	Public Property Status As String
	Public Property EsrData As String


End Class


''' <summary>
''' Klasse für einen ESR Record.
''' </summary>
Public Class EsrRecord

#Region "Public Enums"

	Public Enum BookingTypeEnum
		Storno
		Correction
		Other
	End Enum

#End Region

#Region "Public Constants"

	''' <summary>
	''' Maximale Anzahl Zeichen pro Linie.
	''' </summary>
	''' <remarks></remarks>
	Public Const LINELENGTH_MAX As Integer = 126

#End Region ' Public Constants

#Region "Private Fields"

	Public Enum PaymentProcessState
		Unprocessed
		InProcessing
		Processed
		Question
		Failed
		Lower
		Higher
	End Enum

	Private m_data As String
	Private m_use7digits As Boolean
	Private m_isSummaryRecord As Boolean?
	Private m_bookingType As BookingTypeEnum?
	Private m_KontoNo As String
	Private m_customerNo As Decimal?
	Private m_invoiceNo As Decimal?
	Private m_amount As Decimal?
	Private m_valutaDate As Date?
	Private m_valutaDateDefined As Boolean
	Public Property amountDecision As PaymentProcessState


#End Region ' Private Fields

#Region "Public Methods"

	Public Property paymentNumber As Integer?

	''' <summary>
	''' Konstruktor.
	''' </summary>
	Public Sub New(ByRef data As String, Optional ByVal use7digits As Boolean = False)
		m_data = data
		m_use7digits = use7digits
	End Sub

#End Region ' Public Methods

#Region "Public Properties"

	''' <summary>
	''' Rohdaten
	''' </summary>
	Public ReadOnly Property Data As String
		Get
			Return m_data
		End Get
	End Property

	''' <summary>
	''' 7-stellige Nummern für CustomerNo und RecordNo verwenden.
	''' </summary>
	Public ReadOnly Property Use7Digits As Boolean
		Get
			Return m_use7digits
		End Get
	End Property

	Public ReadOnly Property IsSummaryRecord As Boolean
		Get
			If Not m_isSummaryRecord.HasValue Then
				m_isSummaryRecord = m_data.StartsWith("99")
			End If
			Return m_isSummaryRecord.Value
		End Get
	End Property

	''' <summary>
	''' Buchungstyp.
	''' </summary>
	Public ReadOnly Property BookingType As BookingTypeEnum
		Get
			If Not m_bookingType.HasValue Then
				If m_data.Length >= 3 Then
					Dim value = m_data.Substring(2, 1)
					Select Case value
						Case "5"
							m_bookingType = BookingTypeEnum.Storno
						Case "8"
							m_bookingType = BookingTypeEnum.Correction
						Case Else
							m_bookingType = BookingTypeEnum.Other
					End Select
				Else
					m_bookingType = BookingTypeEnum.Other
				End If
			End If
			Return m_bookingType.Value
		End Get
	End Property

	''' <summary>
	''' Rechnungsnummer.
	''' </summary>
	Public ReadOnly Property KontoNo As String
		Get
			If m_KontoNo Is Nothing Then
				Dim value As String = String.Empty
				If Not Me.IsSummaryRecord Then
					value = m_data.Substring(3, 9)
				End If
				m_KontoNo = value
			End If
			Return m_KontoNo
		End Get
	End Property

	''' <summary>
	''' Kundennummer.
	''' </summary>
	Public ReadOnly Property CustomerNo As Integer
		Get
			If m_customerNo Is Nothing Then
				Dim value As Integer = 0D
				If Not Me.IsSummaryRecord Then
					If m_use7digits Then
						If m_data.Length >= 27 Then
							Integer.TryParse(m_data.Substring(19, 7), value)
						End If
					Else
						If m_data.Length >= 29 Then
							Integer.TryParse(m_data.Substring(18, 10), value)
						End If
					End If
				End If
				m_customerNo = value
			End If
			Return m_customerNo.Value
		End Get
	End Property

	''' <summary>
	''' Rechnungsnummer.
	''' </summary>
	Public ReadOnly Property InvoiceNo As Integer
		Get
			If m_invoiceNo Is Nothing Then
				Dim value As Integer = 0D
				If Not Me.IsSummaryRecord Then
					If m_use7digits Then
						If m_data.Length >= 37 Then
							If Val(m_data.Substring(28, 10)) <= Integer.MaxValue Then
								Integer.TryParse(m_data.Substring(29, 7), value)
							End If
						End If
					Else
						If m_data.Length >= 39 Then
							If Val(m_data.Substring(28, 10)) <= Integer.MaxValue Then
								Integer.TryParse(m_data.Substring(28, 10), value)
							End If
						End If
						End If
				End If
				m_invoiceNo = value
			End If
			Return m_invoiceNo.Value
		End Get
	End Property

	''' <summary>
	''' Betrag oder Gesamtbetrag.
	''' </summary>
	Public ReadOnly Property Amount As Decimal
		Get
			If Not m_amount.HasValue Then
				Dim value As Decimal = 0
				If Me.IsSummaryRecord Then
					If m_data.Length >= 52 Then
						Decimal.TryParse(m_data.Substring(39, 12), value)
					End If
				Else
					If m_data.Length >= 50 Then
						Decimal.TryParse(m_data.Substring(39, 10), value)
					End If
				End If
				m_amount = value / 100
			End If
			Return m_amount.Value
		End Get
	End Property

	''' <summary>
	''' Valuta Datum.
	''' </summary>
	Public ReadOnly Property ValutaDate As Date?
		Get
			If Not m_valutaDateDefined Then
				If Not Me.IsSummaryRecord Then
					Dim year As Integer = 0
					Dim month As Integer = 0
					Dim day As Integer = 0
					If m_data.Length >= 78 Then
						Decimal.TryParse(m_data.Substring(71, 2), year)
						Decimal.TryParse(m_data.Substring(73, 2), month)
						Decimal.TryParse(m_data.Substring(75, 2), day)
					End If
					Try
						m_valutaDate = New DateTime(year + 2000, month, day)
					Catch ex As Exception
						' Nothing
					End Try
				End If
				m_valutaDateDefined = True
			End If
			Return m_valutaDate
		End Get
	End Property

#End Region ' Public Properties

End Class
