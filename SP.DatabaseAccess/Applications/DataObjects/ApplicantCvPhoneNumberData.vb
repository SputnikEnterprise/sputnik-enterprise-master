Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvPhoneNumber. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvPhoneNumberData

    Public Property ID As Integer
    Public Property FK_CvPersonal As Integer
    Public Property FK_CvPhoneNumberType As Integer
    Public Property PhoneNumber As String

  End Class

End Namespace
