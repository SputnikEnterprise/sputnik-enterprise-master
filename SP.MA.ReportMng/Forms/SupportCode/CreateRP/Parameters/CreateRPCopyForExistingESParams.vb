Public Class CreateRPCopyForExistingESParams

  Public Property ESNr As Integer

  Public Property Month_OfReportToCopy As Byte
  Public Property Year_OfReportToCopy As String
  Public Property VonDate_OfReportToCopy As DateTime
  Public Property BisDate_OfReportToCopy As DateTime
  Public Property Suva_OfReprotToCopy As String
  Public Property Kst_OfReportToCopy As String
  Public Property Kst1_OfReportToCopy As String
  Public Property Kst2_OfReportToCopy As String
  Public Property KDBranche_OfReportToCopy As String
  Public Property MDNr As Integer
  Public Property RPNumberOffset As Integer
  Public Property UserName As String

  Public Property NewRPNrOutput As Integer?

  '---Result Values ---

  Public Property NewIdRPOutput As Integer?
  Public Property ResultCode As CreateRPCopyForExistingESResult

  Enum CreateRPCopyForExistingESResult
    ResultSuccess = 1
    ResultNoMoreReportsAllowed = 2
    ResultReportForNextMonthIsAlreadyExisting = 3
    ResultFailure = 4
  End Enum

End Class
