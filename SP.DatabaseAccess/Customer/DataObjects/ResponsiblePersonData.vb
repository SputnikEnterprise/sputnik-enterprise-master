Namespace Customer.DataObjects

  ''' <summary>
  '''Responsible person data.
  ''' </summary>
  Public Class ResponsiblePersonData

		Public Property ID As Integer
		Public Property CustomerNumber As Integer?
		Public Property RecordNumber
		Public Property Position As String
		Public Property Department As String
		Public Property Salutation As String
		Public Property Firstname As String
		Public Property Lastname As String
		Public Property Telephone As String
		Public Property Telefax As String
		Public Property MobilePhone As String

		Public Property Email As String
		Public Property Xing As String
		Public Property ZState1 As String
		Public Property ZState2 As String
		Public Property BirthDate As Date?
		Public Property Interests As String
		Public Property Comments As String

		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String


		' ---Additional transient data (data is not saved/updated, etc.)---
		Public Property TranslatedSalutation As String
		Public Property TranslatedZHowKontakt As String
		Public Property TranslatedZState1 As String
		Public Property TranslatedZState2 As String

	End Class

End Namespace