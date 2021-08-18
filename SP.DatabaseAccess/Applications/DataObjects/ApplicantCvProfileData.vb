Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvProfile. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvProfileData

    Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property TrxmlID As Integer?
		Public Property BusinessBranch As String
		Public Property FK_CvPersonal As Integer?
		Public Property FK_CvDocumentText As Integer?
    Public Property FK_CvDocumentHtml As Integer?
    Public Property CreatedOn As DateTime
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String

  End Class

End Namespace
