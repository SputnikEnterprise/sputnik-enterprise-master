''' <summary>
'''Responsible person data.
''' </summary>
Public Class ResponsiblePersonData

  'ID
  Public Property ID As Integer

  Public Property CustomerNumber As Integer?
  Public Property RecordNumber As Integer?

  Public Property Position As String

  Public Property Department As String
  Public Property Salutation As String

  Public Property Firstname As String
  Public Property Lastname As String
  Public Property LastNameFirstName As String
  Public Property Telephone As String

  Public Property Telefax As String

  Public Property MobilePhone As String
  Public Property Email As String

  Public Property fstate As String
  Public Property sstate As String
  Public Property howcontact As String

  ' ---Additional transient data (data is not saved/updated, etc.)---
  Public Property TranslatedSalutation As String

End Class
