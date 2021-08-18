Namespace ES.DataObjects.ESMng

  ''' <summary>
  ''' ES employee and customer LA data. (ES_MA_LA and ES_KD_LA)
  ''' </summary>
  Public Class ESEmployeeAndCustomerLAData
    Public Property ID As Integer
    Public Property ESNr As Integer?
    Public Property CustomerNumber As Integer?
    Public Property EmployeeNumber As Integer?
    Public Property ESLANr As Integer?
    Public Property KSTNr As Integer?
    Public Property LANr As Decimal?
    Public Property LABez As String
    Public Property Betrag As Decimal
    Public Property Ansatz As Decimal
    Public Property Basis As Decimal
    Public Property Tag As Boolean?
    Public Property Monat As Boolean?
    Public Property Std As Boolean?
    Public Property Kilometer As Boolean?
    Public Property Week As Boolean?

    Public Property Vertrag As Boolean?
    Public Property Currency As String
    Public Property Result As String
    Public Property ESLohnNr As Integer?

  End Class

End Namespace
