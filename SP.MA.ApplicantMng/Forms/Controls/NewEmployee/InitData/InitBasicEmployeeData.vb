Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Customer.DataObjects

Namespace UI

  ''' <summary>
  ''' Initial employee basic data.
  ''' </summary>
  Public Class InitEmployeeBasicData

    Public Property Lastname As String
    Public Property Firstname As String
    Public Property Street As String
    Public Property CountryCode As String
    Public Property PostCode As String
    Public Property Location As String
    Public Property Gender As String
    Public Property Nationality As String
    Public Property CivilState As String
    Public Property Birthdate As DateTime
    Public Property Language As String

  End Class

End Namespace
