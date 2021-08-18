Namespace AdvancePaymentMng.DataObjects

  Public Class NegativeLMData
    Public Property LMNR As Integer?
    Public Property MANR As Integer?
    Public Property KST As String
    Public Property Jahr_Bis As String
    Public Property Jahr_Von As String
    Public Property LANr As Decimal?
		Public Property LP_Von As Integer?
		Public Property LP_Bis As Integer?
    Public Property M_Btr As Decimal?
    Public Property Suva As String
    Public Property LAName As String
    Public Property CreatedFrom As String
    Public Property CreatedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property ChangedOn As DateTime?

    Public ReadOnly Property MonatJahrVon
      Get
        Return String.Format("{0}/{1}", LP_Von, Jahr_Von)
      End Get
    End Property

    Public ReadOnly Property MonatJahrBis
      Get
        Return String.Format("{0}/{1}", LP_Bis, Jahr_Bis)
      End Get
    End Property

  End Class

End Namespace
