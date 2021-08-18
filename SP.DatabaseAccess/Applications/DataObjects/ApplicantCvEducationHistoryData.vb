Imports System.Globalization

Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvEducationHistory. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvEducationHistoryData

    Public Property ID As Integer
    Public Property FK_CvProfile As Integer
    Public Property FK_CvEducation As Integer?
    Public Property FK_CvEducationLevel As Integer?
    Public Property FK_CvEducationDetail As Integer?
    Public Property DegreeDirection As String
    Public Property FK_CvDegreeDirection As Integer?
    Public Property StartDate As DateTime?
    Public Property EndDate As DateTime?
    Public Property InstituteNameAndPlace As String
    Public Property InstituteName As String
    Public Property InstitutePlace As String
    Public Property FK_CvInstituteType As Integer?
    Public Property FK_CvDiploma As Integer?
    Public Property DiplomaDate As DateTime?
    Public Property Subjects As String
    Public Property IsHighestItem As Boolean

  End Class

End Namespace
