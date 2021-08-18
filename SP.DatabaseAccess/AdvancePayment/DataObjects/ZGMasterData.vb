Namespace AdvancePaymentMng.DataObjects

  ''' <summary>
  ''' Table ZG data.
  ''' </summary>
  Public Class ZGMasterData
    Public Property ID As Integer
    Public Property RPNR As Integer?
    Public Property ZGNr As Integer?
    Public Property MANR As Integer?
    Public Property LANR As Integer?
    Public Property LONR As Integer?
    Public Property VGNR As Integer?
    Public Property ZGGRUND As String
    Public Property Betrag As Decimal?
    Public Property Anzahl As Integer?
    Public Property Ansatz As Integer?
    Public Property Basis As Decimal?
    Public Property Currency As String
		Public Property LP As Integer?
    Public Property JAHR As String
    Public Property Aus_Dat As DateTime?
    Public Property ClearingNr As Integer?
    Public Property Bank As String
    Public Property KontoNr As String
    Public Property BankOrt As String
    Public Property DTAAdr1 As String
    Public Property DTAAdr2 As String
    Public Property DTAAdr3 As String
    Public Property DTAAdr4 As String
    Public Property N2Char As String
    Public Property _1000000 As String
    Public Property _100000 As String
    Public Property _10000 As String
    Public Property _1000 As String
    Public Property _100 As String
    Public Property _10 As String
    Public Property _1 As String
    Public Property USName As String
    Public Property Result As String
    Public Property CheckNumber As String
    Public Property GebAbzug As Boolean?
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property DTA_Dat As DateTime?
    Public Property BnkAU As Boolean?
    Public Property DTADate As DateTime?
    Public Property IBANNr As String
    Public Property Swift As String
    Public Property BLZ As String
    Public Property Printed_Dat As DateTime?
    Public Property MDNr As Integer
    Public Property IsCreatedWithLO As Boolean?
    Public Property IsMonthClosed As Boolean
		Public Property EmployeeLastname As String
		Public Property EmployeeFirstname As String


		Public ReadOnly Property EmployeeFullname() As String
			Get
				Return String.Format("{0} {1}", EmployeeFirstname, EmployeeLastname)
			End Get
		End Property

		Public ReadOnly Property EmployeeFullnameWithComma() As String
			Get
				Return String.Format("{1}, {0}", EmployeeFirstname, EmployeeLastname)
			End Get
		End Property

		Public ReadOnly Property Period As String
			Get
				Return String.Format("{0:00} - {1}", LP, JAHR)
			End Get
		End Property


	End Class

End Namespace
