Imports SPS.ExternalServices.DeltavistaWebService

Public Interface IDeltavistaServices

    ''' <summary>
    ''' Requests a solvency report for a customer or company.
    ''' </summary>
    ''' <param name="requestData">The request data.</param>
    ''' <returns>The repsonse data.</returns>
    Function RequestSolvencyReportInformation(ByVal requestData As SolvencyReportRequestDataAbs) As TypeGetReportResponse

    ''' <summary>
    ''' Requests an archived solvency report.
    ''' </summary>
    ''' <param name="requestData">The request data.</param>
    ''' <returns>The repsonse data.</returns>
    Function RequestArchivedSolvencyReportInformation(ByVal requestData As SolvencyReportArchivedRequestData) As TypeGetArchivedReportResponse

    ''' <summary>
    ''' Request debitor details.
    ''' </summary>
    ''' <param name="requestData">The request data.</param>
    ''' <returns>The response data.</returns>
    Function RequestDebtDetails(ByVal requestData As DeptDetailsRequestData) As TypeGetDebtDetailsResponse

End Interface
