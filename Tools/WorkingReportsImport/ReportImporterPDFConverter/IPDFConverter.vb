'------------------------------------
' File: IPDFConverter.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' PDF converter interface.
''' </summary>
Public Interface IPDFConverter

    ''' <summary>
    ''' Converts a (multipage) pdf report.
    ''' </summary>
    ''' <param name="workingReportPath">The working report path.</param>
    ''' <param name="mandantGuid">The mandant guid.</param>
    Sub ConvertPDFReport(ByVal workingReportPath As String, ByVal mandantGuid As String)

End Interface
