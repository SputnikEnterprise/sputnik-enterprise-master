Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvTransportation. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvTransportationData

    Public Property ID As Integer
    Public Property FK_CvCustomArea As Integer
    Public Property DriversLicence As String
    Public Property Car As Boolean
    Public Property Motorcycle As Boolean
    Public Property Bicycle As Boolean

  End Class

End Namespace
