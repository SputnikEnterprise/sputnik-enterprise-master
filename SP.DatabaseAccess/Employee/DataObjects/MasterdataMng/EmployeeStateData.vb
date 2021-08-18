Imports SP.DatabaseAccess.Common

Namespace Employee.DataObjects.MasterdataMng

  Public Class EmployeeStateData

    Public Property ID As Integer
    Public Property GetField As String
    Public Property Description As String
    Public Property Result As String ' Only for MA_State2

    ''' <summary>
    ''' Gets or sets the translated state.
    ''' </summary>
    ''' <returns>Returns the translated state text.</returns>
    Public Property TranslatedState As String

  End Class

End Namespace
