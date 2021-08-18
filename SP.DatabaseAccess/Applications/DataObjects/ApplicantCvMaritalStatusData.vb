Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvMaritalStatus. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvMaritalStatusData

    ''' <summary>
    ''' Enum korrespondierend zu tbl_CvMaritalStatus.ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum MaritalStatusEnum As Integer
      Married = 1
      Unmarried = 2
      LivingTogether = 3
      Widowed = 4
      Divorced = 5
      RegisteredPartnership = 6
    End Enum

    Public Property ID As Integer
    Public Property Name As String
    Public Property Code As String
    Public Property Description As String

  End Class

End Namespace
