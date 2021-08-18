Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects

Namespace AdvancePaymentMng

  ''' <summary>
  ''' Interface for Advance Payment (Vorschuss) database access.
  ''' </summary>
  Public Interface IAdvancePaymentDatabaseAccess

    Function LoadZGMasterData(ByVal zgNr As Integer) As ZGMasterData
		Function LoadAssignedZGMasterData(ByVal zgNumbers As List(Of Integer)) As IEnumerable(Of ZGMasterData)
		Function LoadEmployeeData() As IEnumerable(Of EmployeeData)
		Function LoadGuthabenValuesForAdvancePayment(ByVal mdNumber As Integer, ByVal employeeNumber As Integer, ByVal month As Integer, ByVal year As Integer) As BalanceValues
    Function LoadLAData() As IEnumerable(Of LAData)
    Function LoadInvalidMonthForAdvancePayment(ByVal year As Integer, ByVal employeeNumber As Integer, ByVal mdNumber As Integer) As IEnumerable(Of Integer)
    Function LoadNumberOfESOfEmployeeForMonth(ByVal year As Integer, ByVal month As Integer, ByVal employeeNumber As Integer, ByVal mdNumber As Integer) As Integer?
    Function LoadNegativeLMData(ByVal employeeNumber As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of NegativeLMData)
    Function LoadPaymentReasonTexts(ByVal employeeNumber As Integer, ByVal mdNumber As Integer) As IEnumerable(Of PaymentReasonData)
    Function AddNewZGData(ByVal zgData As ZGMasterData, ByVal zgNumberOffset As Integer) As Boolean
    Function UpdateZGData(ByVal zgData As ZGMasterData) As Boolean
    Function DeleteZGData(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteZGResult

  End Interface

  ''' <summary>
  ''' Result of ZG deletion.
  ''' </summary>
  Public Enum DeleteZGResult
    ResultDeleteOk = 1
    ResultCanNotDeleteBecauseOfLO = 2
    ResultDeleteError = 3
  End Enum

End Namespace