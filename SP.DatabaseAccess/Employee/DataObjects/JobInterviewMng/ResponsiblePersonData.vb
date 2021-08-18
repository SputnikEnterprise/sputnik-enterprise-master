Namespace Employee.DataObjects.JobInterviewMng

  ''' <summary>
  ''' Responsible person data (KD_Zustaendig).
  ''' </summary>
  Public Class ResponsiblePersonData
    Public Property ID As Integer
    Public Property RecordNumber As Integer?
    Public Property CustomerNumber As Integer?
    Public Property TranslatedAnrede As String
    Public Property Lastname As String
    Public Property Firstname As String
    Public Property Postcode As String
    Public Property Location As String
    Public Property Telephone As String
    Public Property Telefax As String
    Public Property EMail As String
    Public Property Homepage As String

		Public Property ZState1 As String
		Public Property ZState2 As String

		Public Property TranslatedSalutation As String
		Public Property TranslatedZHowKontakt As String
		Public Property TranslatedZState1 As String
		Public Property TranslatedZState2 As String

	End Class

End Namespace