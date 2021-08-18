
Namespace Employee.DataObjects.AdvancedPaymentMng
  ''' <summary>
  ''' Employee Payment data (ZG).
  ''' </summary>
  Public Class EmployeeAdvancedPaymentData

    Public Property ID As Integer
    Public Property paymentnumber As Integer?
    Public Property salarynumber As Integer?
    Public Property reportnumber As Integer?
    Public Property employeenumber As Integer?
    Public Property translatedLAname As String

    Public Property paymentdate As DateTime?
    Public Property paymentmonth As Integer
    Public Property paymentyear As Integer
    Public Property Amount As Decimal?
    Public Property createdon As DateTime?
    Public Property createdfrom As String

    Public Property paymentreason As String

  End Class

End Namespace

