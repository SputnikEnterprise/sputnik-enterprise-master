Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports System.Text
'Imports System.Transactions

Namespace Applicant

  ''' <summary>
  ''' Implementation des CV Datenbank Zugriffs.
  ''' </summary>
  ''' <remarks></remarks>
  Public Class CvlDatabaseAccess

    Inherits DatabaseAccessBase
    Implements IAppCvDatabaseAccess

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="translationLanguage">The translation language.</param>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
      MyBase.New(connectionString, translationLanguage)

    End Sub

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="translationLanguage">The translation language string.</param>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
      MyBase.New(connectionString, translationLanguage)
    End Sub

#End Region

#Region "public methodes"

		Public Function AddCvAddressData(ByRef cvAddressData As ApplicantCvAddressData) As Boolean Implements IAppCvDatabaseAccess.AddCvAddressData
      Dim success As Boolean
      Dim sql = "[CreateCvAddress]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@AddressLine", ReplaceMissing(cvAddressData.AddressLine, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@StreetName", ReplaceMissing(cvAddressData.StreetName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@StreetNumberBase", ReplaceMissing(cvAddressData.StreetNumberBase, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@StreetNumberExtension", ReplaceMissing(cvAddressData.StreetNumberExtension, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PostalCode", ReplaceMissing(cvAddressData.PostalCode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@City", ReplaceMissing(cvAddressData.City, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvRegion", ReplaceMissing(cvAddressData.FK_CvRegion, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvCountry", ReplaceMissing(cvAddressData.FK_CvCountry, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvAddressData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvComputerSkillData(ByRef cvComputerSkillData As ApplicantCvComputerSkillData) As Boolean Implements IAppCvDatabaseAccess.AddCvComputerSkillData
      Dim success As Boolean
      Dim sql = "[CreateCvComputerSkill]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvSkill", cvComputerSkillData.FK_CvSkill))
      listOfParams.Add(New SqlClient.SqlParameter("@Text", ReplaceMissing(cvComputerSkillData.Text, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvComputerSkillType", ReplaceMissing(cvComputerSkillData.FK_CvComputerSkillType, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Duration", ReplaceMissing(cvComputerSkillData.Duration, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvComputerSkillData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvComputerSkillTypeData(ByRef cvComputerSkillTypeData As ApplicantCvComputerSkillTypeData) As Boolean Implements IAppCvDatabaseAccess.AddCvComputerSkillTypeData
      Dim success As Boolean
      Dim sql = "[CreateCvComputerSkillType]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(cvComputerSkillTypeData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Code", cvComputerSkillTypeData.Code))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvComputerSkillTypeData.Description, DBNull.Value)))

      ' Output Parameters
      Dim idParameter = New SqlClient.SqlParameter("@Id", SqlDbType.Int)
      idParameter.Direction = ParameterDirection.Output
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not idParameter.Value Is Nothing Then
        cvComputerSkillTypeData.ID = CType(idParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvCustomAreaData(ByRef cvCustomAreaData As ApplicantCvCustomAreaData) As Boolean Implements IAppCvDatabaseAccess.AddCvCustomAreaData
      Dim success As Boolean
      Dim sql = "[CreateCvCustomArea]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvProfile", cvCustomAreaData.FK_CvProfile))
      listOfParams.Add(New SqlClient.SqlParameter("@CvTitle", ReplaceMissing(cvCustomAreaData.CvTitle, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(cvCustomAreaData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedBy", ReplaceMissing(cvCustomAreaData.CreatedBy, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@EditedOn", ReplaceMissing(cvCustomAreaData.EditedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@EditedBy", ReplaceMissing(cvCustomAreaData.EditedBy, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@TotalExperienceYears", ReplaceMissing(cvCustomAreaData.TotalExperienceYears, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CurrentJob", ReplaceMissing(cvCustomAreaData.CurrentJob, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CurrentEmployer", ReplaceMissing(cvCustomAreaData.CurrentEmployer, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Last3Experiences", ReplaceMissing(cvCustomAreaData.Last3Experiences, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvHighestEducationLevel", ReplaceMissing(cvCustomAreaData.FK_CvHighestEducationLevel, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvSalary", ReplaceMissing(cvCustomAreaData.FK_CvSalary, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvProfileStatus", ReplaceMissing(cvCustomAreaData.FK_CvProfileStatus, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvAvailability", ReplaceMissing(cvCustomAreaData.FK_CvAvailability, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CvComment", ReplaceMissing(cvCustomAreaData.CvComment, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LearnedOccupation", ReplaceMissing(cvCustomAreaData.LearnedOccupation, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvApproval", ReplaceMissing(cvCustomAreaData.FK_CvApproval, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@POBox", ReplaceMissing(cvCustomAreaData.POBox, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ExternalID", ReplaceMissing(cvCustomAreaData.ExternalID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvProfilePicture", ReplaceMissing(cvCustomAreaData.FK_CvProfilePicture, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvCustomAreaData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvDegreeDirectionData(ByRef cvDegreeDirectionData As ApplicantCvDegreeDirectionData) As Boolean Implements IAppCvDatabaseAccess.AddCvDegreeDirectionData
      Dim success As Boolean
      Dim sql = "[CreateCvDegreeDirection]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(cvDegreeDirectionData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Code", cvDegreeDirectionData.Code))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvDegreeDirectionData.Description, DBNull.Value)))

      ' Output Parameters
      Dim idParameter = New SqlClient.SqlParameter("@Id", SqlDbType.Int)
      idParameter.Direction = ParameterDirection.Output
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not idParameter.Value Is Nothing Then
        cvDegreeDirectionData.ID = CType(idParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvDocumentHtmlData(ByRef cvDocumentHtmlData As ApplicantCvDocumentHtmlData) As Boolean Implements IAppCvDatabaseAccess.AddCvDocumentHtmlData
      Dim success As Boolean
      Dim sql = "[CreateCvDocumentHtml]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Content", ReplaceMissing(cvDocumentHtmlData.Content, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvDocumentHtmlData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvDocumentTextData(ByRef cvDocumentTextData As ApplicantCvDocumentTextData) As Boolean Implements IAppCvDatabaseAccess.AddCvDocumentTextData
      Dim success As Boolean
      Dim sql = "[CreateCvDocumentText]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Content", ReplaceMissing(cvDocumentTextData.Content, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvDocumentTextData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvDriversLicenceData(ByRef cvDriversLicenceData As ApplicantCvDriversLicenceData) As Boolean Implements IAppCvDatabaseAccess.AddCvDriversLicenceData
      Dim success As Boolean
      Dim sql = "[CreateCvDriversLicence]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(cvDriversLicenceData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Code", cvDriversLicenceData.Code))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvDriversLicenceData.Description, DBNull.Value)))

      ' Output Parameters
      Dim idParameter = New SqlClient.SqlParameter("@Id", SqlDbType.Int)
      idParameter.Direction = ParameterDirection.Output
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not idParameter.Value Is Nothing Then
        cvDriversLicenceData.ID = CType(idParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvEducationData(ByRef cvEducationData As ApplicantCvEducationData) As Boolean Implements IAppCvDatabaseAccess.AddCvEducationData
      Dim success As Boolean
      Dim sql = "[CreateCvEducation]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(cvEducationData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Code", cvEducationData.Code))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvEducationData.Description, DBNull.Value)))

      ' Output Parameters
      Dim idParameter = New SqlClient.SqlParameter("@Id", SqlDbType.Int)
      idParameter.Direction = ParameterDirection.Output
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not idParameter.Value Is Nothing Then
        cvEducationData.ID = CType(idParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvEducationDetailData(ByRef cvEducationDetailData As ApplicantCvEducationDetailData) As Boolean Implements IAppCvDatabaseAccess.AddCvEducationDetailData
      Dim success As Boolean
      Dim sql = "[CreateCvEducationDetail]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(cvEducationDetailData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Code", cvEducationDetailData.Code))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvEducationDetailData.Description, DBNull.Value)))

      ' Output Parameters
      Dim idParameter = New SqlClient.SqlParameter("@Id", SqlDbType.Int)
      idParameter.Direction = ParameterDirection.Output
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not idParameter.Value Is Nothing Then
        cvEducationDetailData.ID = CType(idParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvEducationHistoryData(ByRef cvEducationHistoryData As ApplicantCvEducationHistoryData) As Boolean Implements IAppCvDatabaseAccess.AddCvEducationHistoryData
      Dim success As Boolean
      Dim sql = "[CreateCvEducationHistory]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvProfile", cvEducationHistoryData.FK_CvProfile))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvEducation", ReplaceMissing(cvEducationHistoryData.FK_CvEducation, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvEducationLevel", ReplaceMissing(cvEducationHistoryData.FK_CvEducationLevel, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvEducationDetail", ReplaceMissing(cvEducationHistoryData.FK_CvEducationDetail, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DegreeDirection", ReplaceMissing(cvEducationHistoryData.DegreeDirection, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvDegreeDirection", ReplaceMissing(cvEducationHistoryData.FK_CvDegreeDirection, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@StartDate", ReplaceMissing(cvEducationHistoryData.StartDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@EndDate", ReplaceMissing(cvEducationHistoryData.EndDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@InstituteNameAndPlace", ReplaceMissing(cvEducationHistoryData.InstituteNameAndPlace, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@InstituteName", ReplaceMissing(cvEducationHistoryData.InstituteName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@InstitutePlace", ReplaceMissing(cvEducationHistoryData.InstitutePlace, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvInstituteType", ReplaceMissing(cvEducationHistoryData.FK_CvInstituteType, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvDiploma", ReplaceMissing(cvEducationHistoryData.FK_CvDiploma, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DiplomaDate", ReplaceMissing(cvEducationHistoryData.DiplomaDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Subjects", ReplaceMissing(cvEducationHistoryData.Subjects, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IsHighestItem", ReplaceMissing(cvEducationHistoryData.IsHighestItem, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvEducationHistoryData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvEmploymentHistoryData(ByRef cvEmploymentHistoryData As ApplicantCvEmploymentHistoryData) As Boolean Implements IAppCvDatabaseAccess.AddCvEmploymentHistoryData
      Dim success As Boolean
      Dim sql = "[CreateCvEmploymentHistory]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvProfile", cvEmploymentHistoryData.FK_CvProfile))
      listOfParams.Add(New SqlClient.SqlParameter("@JobTitle", ReplaceMissing(cvEmploymentHistoryData.JobTitle, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvJobTitle", ReplaceMissing(cvEmploymentHistoryData.FK_CvJobTitle, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@StartDate", ReplaceMissing(cvEmploymentHistoryData.StartDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@EndDate", ReplaceMissing(cvEmploymentHistoryData.EndDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ExperienceYears", ReplaceMissing(cvEmploymentHistoryData.ExperienceYears, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@EmployerNameAndPlace", ReplaceMissing(cvEmploymentHistoryData.EmployerNameAndPlace, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@EmployerName", ReplaceMissing(cvEmploymentHistoryData.EmployerName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@EmployerPlace", ReplaceMissing(cvEmploymentHistoryData.EmployerPlace, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvEmploymentHistoryData.Description, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@QuitReason", ReplaceMissing(cvEmploymentHistoryData.QuitReason, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IsLastItem", ReplaceMissing(cvEmploymentHistoryData.IsLastItem, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IsLastItemWithJobTitle", ReplaceMissing(cvEmploymentHistoryData.IsLastItemWithJobTitle, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IsCurrentEmployer", ReplaceMissing(cvEmploymentHistoryData.IsCurrentEmployer, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Remarks", ReplaceMissing(cvEmploymentHistoryData.Remarks, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvEmploymentHistoryData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvEmailData(ByRef cvEmailData As ApplicantCvEmailData) As Boolean Implements IAppCvDatabaseAccess.AddCvEmailData
      Dim success As Boolean
      If cvEmailData IsNot Nothing AndAlso Not String.IsNullOrEmpty(cvEmailData.Email) Then
        Dim sql = "[CreateCvEmail]"
        Dim listOfParams = New List(Of SqlClient.SqlParameter)

        ' Input Parameters
        listOfParams.Add(New SqlClient.SqlParameter("@FK_CvPersonal", cvEmailData.FK_CvPersonal))
        listOfParams.Add(New SqlClient.SqlParameter("@FK_CvEmailType", cvEmailData.FK_CvEmailType))
        listOfParams.Add(New SqlClient.SqlParameter("@Email", cvEmailData.Email))

        ' Output Parameters
        Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
        newIdParameter.Direction = ParameterDirection.Output
        listOfParams.Add(newIdParameter)

        success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

        If Not newIdParameter.Value Is Nothing Then
          cvEmailData.ID = CType(newIdParameter.Value, Integer)
        Else
          success = False
        End If
      End If

      Return success
    End Function

    Public Function AddCvExtraInfoData(ByRef cvExtraInfoData As ApplicantCvExtraInfoData) As Boolean Implements IAppCvDatabaseAccess.AddCvExtraInfoData
      Dim success As Boolean
      Dim sql = "[CreateCvExtraInfo]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvCustomArea", cvExtraInfoData.FK_CvCustomArea))
      listOfParams.Add(New SqlClient.SqlParameter("@Key", cvExtraInfoData.Key))
      listOfParams.Add(New SqlClient.SqlParameter("@Value", ReplaceMissing(cvExtraInfoData.Value, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvExtraInfoData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvHobbyData(ByRef cvHobbyData As ApplicantCvHobbyData) As Boolean Implements IAppCvDatabaseAccess.AddCvHobbyData
      Dim success As Boolean
      Dim sql = "[CreateCvHobby]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvOther", cvHobbyData.FK_CvOther))
      listOfParams.Add(New SqlClient.SqlParameter("@Text", ReplaceMissing(cvHobbyData.Text, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvHobbyData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvInstituteTypeData(ByRef cvInstituteTypeData As ApplicantCvInstituteTypeData) As Boolean Implements IAppCvDatabaseAccess.AddCvInstituteTypeData
      Dim success As Boolean
      Dim sql = "[CreateCvInstituteType]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(cvInstituteTypeData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Code", cvInstituteTypeData.Code))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvInstituteTypeData.Description, DBNull.Value)))

      ' Output Parameters
      Dim idParameter = New SqlClient.SqlParameter("@Id", SqlDbType.Int)
      idParameter.Direction = ParameterDirection.Output
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not idParameter.Value Is Nothing Then
        cvInstituteTypeData.ID = CType(idParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvJobTitleData(ByRef cvJobTitleData As ApplicantCvJobTitleData) As Boolean Implements IAppCvDatabaseAccess.AddCvJobTitleData
      Dim success As Boolean
      Dim sql = "[CreateCvJobTitle]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(cvJobTitleData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Code", cvJobTitleData.Code))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvJobTitleData.Description, DBNull.Value)))

      ' Output Parameters
      Dim idParameter = New SqlClient.SqlParameter("@Id", SqlDbType.Int)
      idParameter.Direction = ParameterDirection.Output
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not idParameter.Value Is Nothing Then
        cvJobTitleData.ID = CType(idParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvLanguageSkillData(ByRef cvLanguageSkillData As ApplicantCvLanguageSkillData) As Boolean Implements IAppCvDatabaseAccess.AddCvLanguageSkillData
      Dim success As Boolean
      Dim sql = "[CreateCvLanguageSkill]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvSkill", cvLanguageSkillData.FK_CvSkill))
      listOfParams.Add(New SqlClient.SqlParameter("@Text", ReplaceMissing(cvLanguageSkillData.Text, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvLanguageSkillType", ReplaceMissing(cvLanguageSkillData.FK_CvLanguageSkillType, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvLanguageProficiency", ReplaceMissing(cvLanguageSkillData.FK_CvLanguageProficiency, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IsNativeLanguage", cvLanguageSkillData.IsNativeLanguage))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvLanguageSkillData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvOtherData(ByRef cvOtherData As ApplicantCvOtherData) As Boolean Implements IAppCvDatabaseAccess.AddCvOtherData
      Dim success As Boolean
      Dim sql = "[CreateCvOther]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvProfile", cvOtherData.FK_CvProfile))
      listOfParams.Add(New SqlClient.SqlParameter("@TotalExperience", ReplaceMissing(cvOtherData.TotalExperience, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Salary", ReplaceMissing(cvOtherData.Salary, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Benefits", ReplaceMissing(cvOtherData.Benefits, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvOtherData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvPersonalData(ByRef cvPersonalData As ApplicantCvPersonalData) As Boolean Implements IAppCvDatabaseAccess.AddCvPersonalData
      Dim success As Boolean
      Dim sql = "[CreateCvPersonal]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Initials", ReplaceMissing(cvPersonalData.Initials, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Title", ReplaceMissing(cvPersonalData.Title, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FirstName", ReplaceMissing(cvPersonalData.FirstName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MiddleName", ReplaceMissing(cvPersonalData.MiddleName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LastNamePrefix", ReplaceMissing(cvPersonalData.LastNamePrefix, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LastName", ReplaceMissing(cvPersonalData.LastName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FullName", ReplaceMissing(cvPersonalData.FullName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DateOfBirth", ReplaceMissing(cvPersonalData.DateOfBirth, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PlaceOfBirth", ReplaceMissing(cvPersonalData.PlaceOfBirth, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvNationality", ReplaceMissing(cvPersonalData.FK_CvNationality, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvGender", ReplaceMissing(cvPersonalData.FK_CvGender, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvDriversLicence", ReplaceMissing(cvPersonalData.FK_CvDriversLicence, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvMaritalStatus", ReplaceMissing(cvPersonalData.FK_CvMaritalStatus, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Availability", ReplaceMissing(cvPersonalData.Availability, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MilitaryService", ReplaceMissing(cvPersonalData.MilitaryService, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvAddress", ReplaceMissing(cvPersonalData.FK_CvAddress, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvPersonalData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvPhoneNumberData(ByRef cvPhoneNumberData As ApplicantCvPhoneNumberData) As Boolean Implements IAppCvDatabaseAccess.AddCvPhoneNumberData
      Dim success As Boolean
      If cvPhoneNumberData IsNot Nothing AndAlso Not String.IsNullOrEmpty(cvPhoneNumberData.PhoneNumber) Then
        Dim sql = "[CreateCvPhoneNumber]"
        Dim listOfParams = New List(Of SqlClient.SqlParameter)

        ' Input Parameters
        listOfParams.Add(New SqlClient.SqlParameter("@FK_CvPersonal", cvPhoneNumberData.FK_CvPersonal))
        listOfParams.Add(New SqlClient.SqlParameter("@FK_CVPhoneNumberType", cvPhoneNumberData.FK_CvPhoneNumberType))
        listOfParams.Add(New SqlClient.SqlParameter("@PhoneNumber", cvPhoneNumberData.PhoneNumber))

        ' Output Parameters
        Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
        newIdParameter.Direction = ParameterDirection.Output
        listOfParams.Add(newIdParameter)

        success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

        If Not newIdParameter.Value Is Nothing Then
          cvPhoneNumberData.ID = CType(newIdParameter.Value, Integer)
        Else
          success = False
        End If
      End If

      Return success
    End Function

    Public Function AddCvPictureData(ByRef cvPictureData As ApplicantCvPictureData) As Boolean Implements IAppCvDatabaseAccess.AddCvPictureData
      Dim success As Boolean
      Dim sql = "[CreateCvPicture]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Content", ReplaceMissing(cvPictureData.Content, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Filename", ReplaceMissing(cvPictureData.Filename, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ContentType", ReplaceMissing(cvPictureData.ContentType, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvPictureData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvProfileData(ByRef cvProfileData As ApplicantCvProfileData) As Boolean Implements IAppCvDatabaseAccess.AddCvProfileData
      Dim success As Boolean
      Dim sql = "[CreateCvProfile]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@TrxmlID", ReplaceMissing(cvProfileData.TrxmlID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvPersonal", ReplaceMissing(cvProfileData.FK_CvPersonal, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvDocumentText", ReplaceMissing(cvProfileData.FK_CvDocumentText, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvDocumentHtml", ReplaceMissing(cvProfileData.FK_CvDocumentHtml, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvProfileData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvReferenceData(ByRef cvReferenceData As ApplicantCvReferenceData) As Boolean Implements IAppCvDatabaseAccess.AddCvReferenceData
      Dim success As Boolean
      Dim sql = "[CreateCvReference]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvOther", cvReferenceData.FK_CvOther))
      listOfParams.Add(New SqlClient.SqlParameter("@Text", ReplaceMissing(cvReferenceData.Text, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvReferenceData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvSocialMediaData(ByRef cvSocialMediaData As ApplicantCvSocialMediaData) As Boolean Implements IAppCvDatabaseAccess.AddCvSocialMediaData
      Dim success As Boolean
      If cvSocialMediaData IsNot Nothing AndAlso Not String.IsNullOrEmpty(cvSocialMediaData.Url) Then
        Dim sql = "[CreateCvSocialMedia]"
        Dim listOfParams = New List(Of SqlClient.SqlParameter)

        ' Input Parameters
        listOfParams.Add(New SqlClient.SqlParameter("@FK_CvPersonal", cvSocialMediaData.FK_CvPersonal))
        listOfParams.Add(New SqlClient.SqlParameter("@FK_CvSocialMediaType", cvSocialMediaData.FK_CvSocialMediaType))
        listOfParams.Add(New SqlClient.SqlParameter("@Url", cvSocialMediaData.Url))

        ' Output Parameters
        Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
        newIdParameter.Direction = ParameterDirection.Output
        listOfParams.Add(newIdParameter)

        success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

        If Not newIdParameter.Value Is Nothing Then
          cvSocialMediaData.ID = CType(newIdParameter.Value, Integer)
        Else
          success = False
        End If
      End If

      Return success
    End Function

    Public Function AddCvSocialMediaTypeData(ByRef cvSocialMediaTypeData As ApplicantCvSocialMediaTypeData) As Boolean Implements IAppCvDatabaseAccess.AddCvSocialMediaTypeData
      Dim success As Boolean
      Dim sql = "[CreateCvSocialMediaType]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(cvSocialMediaTypeData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Code", cvSocialMediaTypeData.Code))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvSocialMediaTypeData.Description, DBNull.Value)))

      ' Output Parameters
      Dim idParameter = New SqlClient.SqlParameter("@Id", SqlDbType.Int)
      idParameter.Direction = ParameterDirection.Output
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not idParameter.Value Is Nothing Then
        cvSocialMediaTypeData.ID = CType(idParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvSkillData(ByRef cvSkillData As ApplicantCvSkillData) As Boolean Implements IAppCvDatabaseAccess.AddCvSkillData
      Dim success As Boolean
      Dim sql = "[CreateCvSkill]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvProfile", cvSkillData.FK_CvProfile))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvSkillData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvSoftSkillData(ByRef cvSoftSkillData As ApplicantCvSoftSkillData) As Boolean Implements IAppCvDatabaseAccess.AddCvSoftSkillData
      Dim success As Boolean
      Dim sql = "[CreateCvSoftSkill]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvSkill", cvSoftSkillData.FK_CvSkill))
      listOfParams.Add(New SqlClient.SqlParameter("@Text", ReplaceMissing(cvSoftSkillData.Text, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvSoftSkillType", ReplaceMissing(cvSoftSkillData.FK_CvSoftSkillType, DBNull.Value)))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvSoftSkillData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function AddCvSoftSkillTypeData(ByRef cvSoftSkillTypeData As ApplicantCvSoftSkillTypeData) As Boolean Implements IAppCvDatabaseAccess.AddCvSoftSkillTypeData
      Dim success As Boolean
      Dim sql = "[CreateCvSoftSkillType]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(cvSoftSkillTypeData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Code", cvSoftSkillTypeData.Code))
      listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(cvSoftSkillTypeData.Description, DBNull.Value)))

      ' Output Parameters
      Dim idParameter = New SqlClient.SqlParameter("@Id", SqlDbType.Int)
      idParameter.Direction = ParameterDirection.Output
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not idParameter.Value Is Nothing Then
        cvSoftSkillTypeData.ID = CType(idParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function DeleteCvProfileData(
      Optional ByVal id As Integer? = Nothing,
      Optional ByVal trxmlID As Integer? = Nothing,
      Optional ByVal deleteRelated As Boolean = False
      ) As Boolean Implements IAppCvDatabaseAccess.DeleteCvProfileData

      Dim success As Boolean
      Dim sql = "[DeleteCvProfile]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      If id.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@ID", id.Value))
      If trxmlID.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@TrxmlID", trxmlID.Value))
      listOfParams.Add(New SqlClient.SqlParameter("@DeleteRelated", deleteRelated))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success
    End Function

    Public Function AddCvTransportationData(ByRef cvTransportationData As ApplicantCvTransportationData) As Boolean Implements IAppCvDatabaseAccess.AddCvTransportationData
      Dim success As Boolean
      Dim sql = "[CreateCvTransportation]"
      Dim listOfParams = New List(Of SqlClient.SqlParameter)

      ' Input Parameters
      listOfParams.Add(New SqlClient.SqlParameter("@FK_CvCustomArea", cvTransportationData.FK_CvCustomArea))
      listOfParams.Add(New SqlClient.SqlParameter("@DriversLicence", ReplaceMissing(cvTransportationData.DriversLicence, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Car", cvTransportationData.Car))
      listOfParams.Add(New SqlClient.SqlParameter("@Motorcycle", cvTransportationData.Motorcycle))
      listOfParams.Add(New SqlClient.SqlParameter("@Bicycle", cvTransportationData.Bicycle))

      ' Output Parameters
      Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing Then
        cvTransportationData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    Public Function GetCvApproval(ByVal code As String) As ApplicantCvApprovalData Implements IAppCvDatabaseAccess.GetCvApproval
      Dim dictionary = CvApprovalDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvAvailability(ByVal code As String) As ApplicantCvAvailabilityData Implements IAppCvDatabaseAccess.GetCvAvailability
      Dim dictionary = CvAvailabilityDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvComputerSkillType(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvComputerSkillTypeData Implements IAppCvDatabaseAccess.GetCvComputerSkillType
      If Not String.IsNullOrEmpty(code) Then
        Dim dictionary = CvComputerSkillTypeDictionary
        If Not dictionary.ContainsKey(code) Then
          AddCvComputerSkillTypeData(New ApplicantCvComputerSkillTypeData(code, name, description))
          s_cvComputerSkillTypeDictionary = Nothing
          dictionary = CvComputerSkillTypeDictionary
        End If
        If dictionary.ContainsKey(code) Then
          Return dictionary(code)
        End If
      End If
      Return Nothing
    End Function

    Public Function GetCvCountry(ByVal code As String) As ApplicantCvCountryData Implements IAppCvDatabaseAccess.GetCvCountry
      Dim dictionary = CvCountryDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvDegreeDirection(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvDegreeDirectionData Implements IAppCvDatabaseAccess.GetCvDegreeDirection
      If Not String.IsNullOrEmpty(code) Then
        Dim dictionary = CvDegreeDirectionDictionary
        If Not dictionary.ContainsKey(code) Then
          AddCvDegreeDirectionData(New ApplicantCvDegreeDirectionData(code, name, description))
          s_cvDegreeDirectionDictionary = Nothing
          dictionary = CvDegreeDirectionDictionary
        End If
        If dictionary.ContainsKey(code) Then
          Return dictionary(code)
        End If
      End If
      Return Nothing
    End Function

    Public Function GetCvDiploma(ByVal code As String) As ApplicantCvDiplomaData Implements IAppCvDatabaseAccess.GetCvDiploma
      Dim dictionary = CvDiplomaDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvDriversLicence(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvDriversLicenceData Implements IAppCvDatabaseAccess.GetCvDriversLicence
      If Not String.IsNullOrEmpty(code) Then
        Dim dictionary = CvDriversLicenceDictionary
        If Not dictionary.ContainsKey(code) Then
          AddCvDriversLicenceData(New ApplicantCvDriversLicenceData(code, name, description))
          s_cvDriversLicenceDictionary = Nothing
          dictionary = CvDriversLicenceDictionary
        End If
        If dictionary.ContainsKey(code) Then
          Return dictionary(code)
        End If
      End If
      Return Nothing
    End Function

    Public Function GetCvEducation(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvEducationData Implements IAppCvDatabaseAccess.GetCvEducation
      If Not String.IsNullOrEmpty(code) Then
        Dim dictionary = CvEducationDictionary
        If Not dictionary.ContainsKey(code) Then
          AddCvEducationData(New ApplicantCvEducationData(code, name, description))
          s_cvEducationDictionary = Nothing
          dictionary = CvEducationDictionary
        End If
        If dictionary.ContainsKey(code) Then
          Return dictionary(code)
        End If
      End If
      Return Nothing
    End Function

    Public Function GetCvEducationDetail(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvEducationDetailData Implements IAppCvDatabaseAccess.GetCvEducationDetail
      If Not String.IsNullOrEmpty(code) Then
        Dim dictionary = CvEducationDetailDictionary
        If Not dictionary.ContainsKey(code) Then
          AddCvEducationDetailData(New ApplicantCvEducationDetailData(code, name, description))
          s_cvEducationDetailDictionary = Nothing
          dictionary = CvEducationDetailDictionary
        End If
        If dictionary.ContainsKey(code) Then
          Return dictionary(code)
        End If
      End If
      Return Nothing
    End Function

    Public Function GetCvEducationLevel(ByVal code As String) As ApplicantCvEducationLevelData Implements IAppCvDatabaseAccess.GetCvEducationLevel
      Dim dictionary = CvEducationLevelDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvEmailType(ByVal code As String) As ApplicantCvEmailTypeData Implements IAppCvDatabaseAccess.GetCvEmailType
      Dim dictionary = CvEmailTypeDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvGender(ByVal code As String) As ApplicantCvGenderData Implements IAppCvDatabaseAccess.GetCvGender
      Dim dictionary = CvGenderDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvHighestEducationLevel(ByVal code As String) As ApplicantCvHighestEducationLevelData Implements IAppCvDatabaseAccess.GetCvHighestEducationLevel
      Dim dictionary = CvHighestEducationLevelDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvInstituteType(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvInstituteTypeData Implements IAppCvDatabaseAccess.GetCvInstituteType
      If Not String.IsNullOrEmpty(code) Then
        Dim dictionary = CvInstituteTypeDictionary
        If Not dictionary.ContainsKey(code) Then
          AddCvInstituteTypeData(New ApplicantCvInstituteTypeData(code, name, description))
          s_cvInstituteTypeDictionary = Nothing
          dictionary = CvInstituteTypeDictionary
        End If
        If dictionary.ContainsKey(code) Then
          Return dictionary(code)
        End If
      End If
      Return Nothing
    End Function

    Public Function GetCvJobTitle(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvJobTitleData Implements IAppCvDatabaseAccess.GetCvJobTitle
      If Not String.IsNullOrEmpty(code) Then
        Dim dictionary = CvJobTitleDictionary
        If Not dictionary.ContainsKey(code) Then
          AddCvJobTitleData(New ApplicantCvJobTitleData(code, name, description))
          s_cvJobTitleDictionary = Nothing
          dictionary = CvJobTitleDictionary
        End If
        If dictionary.ContainsKey(code) Then
          Return dictionary(code)
        End If
      End If
      Return Nothing
    End Function

    Public Function GetCvLanguageSkillType(ByVal code As String) As ApplicantCvLanguageSkillTypeData Implements IAppCvDatabaseAccess.GetCvLanguageSkillType
      Dim dictionary = CvLanguageSkillTypeDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

		Public Function GetCvLanguageSkillTypeByID(ByVal recID As Integer) As List(Of ApplicantCvLanguageSkillTypeData) Implements IAppCvDatabaseAccess.GetCvLanguageSkillTypeByID
			Dim dictionary = CvLanguageSkillTypeList
			Dim listID = (From kp In dictionary
										Where kp.ID = recID).ToList()
			Return listID

		End Function

		Public Function GetCvLanguageProficiency(ByVal code As String) As ApplicantCvLanguageProficiencyData Implements IAppCvDatabaseAccess.GetCvLanguageProficiency
      Dim dictionary = CvLanguageProficiencyDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvMaritalStatus(ByVal code As String) As ApplicantCvMaritalStatusData Implements IAppCvDatabaseAccess.GetCvMaritalStatus
      Dim dictionary = CvMaritalStatusDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvNationality(ByVal code As String) As ApplicantCvNationalityData Implements IAppCvDatabaseAccess.GetCvNationality
      Dim dictionary = CvNationalityDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvPhoneNumberType(ByVal code As String) As ApplicantCvPhoneNumberTypeData Implements IAppCvDatabaseAccess.GetCvPhoneNumberType
      Dim dictionary = CvPhoneNumberTypeDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvProfileStatus(ByVal code As String) As ApplicantCvProfileStatusData Implements IAppCvDatabaseAccess.GetCvProfileStatus
      Dim dictionary = CvProfileStatusDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvRegion(ByVal code As String) As ApplicantCvRegionData Implements IAppCvDatabaseAccess.GetCvRegion
      Dim dictionary = CvRegionDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvSalary(ByVal code As String) As ApplicantCvSalaryData Implements IAppCvDatabaseAccess.GetCvSalary
      Dim dictionary = CvSalaryDictionary
      If Not String.IsNullOrEmpty(code) AndAlso dictionary.ContainsKey(code) Then
        Return dictionary(code)
      Else
        Return Nothing
      End If
    End Function

    Public Function GetCvSocialMediaType(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvSocialMediaTypeData Implements IAppCvDatabaseAccess.GetCvSocialMediaType
      If Not String.IsNullOrEmpty(code) Then
        Dim dictionary = CvSocialMediaTypeDictionary
        If Not dictionary.ContainsKey(code) Then
          AddCvSocialMediaTypeData(New ApplicantCvSocialMediaTypeData(code, name, description))
          s_cvSocialMediaTypeDictionary = Nothing
          dictionary = CvSocialMediaTypeDictionary
        End If
        If dictionary.ContainsKey(code) Then
          Return dictionary(code)
        End If
      End If
      Return Nothing
    End Function

    Public Function GetCvSoftSkillType(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvSoftSkillTypeData Implements IAppCvDatabaseAccess.GetCvSoftSkillType
      If Not String.IsNullOrEmpty(code) Then
        Dim dictionary = CvSoftSkillTypeDictionary
        If Not dictionary.ContainsKey(code) Then
          AddCvSoftSkillTypeData(New ApplicantCvSoftSkillTypeData(code, name, description))
          s_cvSoftSkillTypeDictionary = Nothing
          dictionary = CvSoftSkillTypeDictionary
        End If
        If dictionary.ContainsKey(code) Then
          Return dictionary(code)
        End If
      End If
      Return Nothing
    End Function

#End Region




#Region "Private Methods"

		Private ReadOnly Property CvApprovalDictionary As Dictionary(Of String, ApplicantCvApprovalData)
      Get
        If s_cvApprovalDictionary Is Nothing Then
          s_cvApprovalDictionary = New Dictionary(Of String, ApplicantCvApprovalData)
          Dim sql = "SELECT * FROM tbl_CvApproval ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvApprovalData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvApprovalDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvApprovalDictionary
      End Get
    End Property

    Private ReadOnly Property CvAvailabilityDictionary As Dictionary(Of String, ApplicantCvAvailabilityData)
      Get
        If s_cvAvailabilityDictionary Is Nothing Then
          s_cvAvailabilityDictionary = New Dictionary(Of String, ApplicantCvAvailabilityData)
          Dim sql = "SELECT * FROM tbl_CvAvailability ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvAvailabilityData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvAvailabilityDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvAvailabilityDictionary
      End Get
    End Property

    Private ReadOnly Property CvCountryDictionary As Dictionary(Of String, ApplicantCvCountryData)
      Get
        If s_cvCountryDictionary Is Nothing Then
          s_cvCountryDictionary = New Dictionary(Of String, ApplicantCvCountryData)
          Dim sql = "SELECT * FROM tbl_CvCountry ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvCountryData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvCountryDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvCountryDictionary
      End Get
    End Property

    Private ReadOnly Property CvComputerSkillTypeDictionary As Dictionary(Of String, ApplicantCvComputerSkillTypeData)
      Get
        If s_cvComputerSkillTypeDictionary Is Nothing Then
          s_cvComputerSkillTypeDictionary = New Dictionary(Of String, ApplicantCvComputerSkillTypeData)
          Dim sql = "SELECT * FROM tbl_CvComputerSkillType ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvComputerSkillTypeData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvComputerSkillTypeDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvComputerSkillTypeDictionary
      End Get
    End Property

    Private ReadOnly Property CvDegreeDirectionDictionary As Dictionary(Of String, ApplicantCvDegreeDirectionData)
      Get
        If s_cvDegreeDirectionDictionary Is Nothing Then
          s_cvDegreeDirectionDictionary = New Dictionary(Of String, ApplicantCvDegreeDirectionData)
          Dim sql = "SELECT * FROM tbl_CvDegreeDirection ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvDegreeDirectionData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvDegreeDirectionDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvDegreeDirectionDictionary
      End Get
    End Property

    Private ReadOnly Property CvDiplomaDictionary As Dictionary(Of String, ApplicantCvDiplomaData)
      Get
        If s_cvDiplomaDictionary Is Nothing Then
          s_cvDiplomaDictionary = New Dictionary(Of String, ApplicantCvDiplomaData)
          Dim sql = "SELECT * FROM tbl_CvDiploma ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvDiplomaData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvDiplomaDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvDiplomaDictionary
      End Get
    End Property

    Private ReadOnly Property CvDriversLicenceDictionary As Dictionary(Of String, ApplicantCvDriversLicenceData)
      Get
        If s_cvDriversLicenceDictionary Is Nothing Then
          s_cvDriversLicenceDictionary = New Dictionary(Of String, ApplicantCvDriversLicenceData)
          Dim sql = "SELECT * FROM tbl_CvDriversLicence ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvDriversLicenceData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvDriversLicenceDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvDriversLicenceDictionary
      End Get
    End Property

    Private ReadOnly Property CvEducationDictionary As Dictionary(Of String, ApplicantCvEducationData)
      Get
        If s_cvEducationDictionary Is Nothing Then
          s_cvEducationDictionary = New Dictionary(Of String, ApplicantCvEducationData)
          Dim sql = "SELECT * FROM tbl_CvEducation ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvEducationData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvEducationDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvEducationDictionary
      End Get
    End Property

    Private ReadOnly Property CvEducationDetailDictionary As Dictionary(Of String, ApplicantCvEducationDetailData)
      Get
        If s_cvEducationDetailDictionary Is Nothing Then
          s_cvEducationDetailDictionary = New Dictionary(Of String, ApplicantCvEducationDetailData)
          Dim sql = "SELECT * FROM tbl_CvEducationDetail ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvEducationDetailData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvEducationDetailDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvEducationDetailDictionary
      End Get
    End Property

    Private ReadOnly Property CvEducationLevelDictionary As Dictionary(Of String, ApplicantCvEducationLevelData)
      Get
        If s_cvEducationLevelDictionary Is Nothing Then
          s_cvEducationLevelDictionary = New Dictionary(Of String, ApplicantCvEducationLevelData)
          Dim sql = "SELECT * FROM tbl_CvEducationLevel ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvEducationLevelData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvEducationLevelDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvEducationLevelDictionary
      End Get
    End Property

    Private ReadOnly Property CvEmailTypeDictionary As Dictionary(Of String, ApplicantCvEmailTypeData)
      Get
        If s_cvEmailTypeDictionary Is Nothing Then
          s_cvEmailTypeDictionary = New Dictionary(Of String, ApplicantCvEmailTypeData)
          Dim sql = "SELECT * FROM tbl_CvPhoneNumberType ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvEmailTypeData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvEmailTypeDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvEmailTypeDictionary
      End Get
    End Property

    Private ReadOnly Property CvGenderDictionary As Dictionary(Of String, ApplicantCvGenderData)
      Get
        If s_cvGenderDictionary Is Nothing Then
          s_cvGenderDictionary = New Dictionary(Of String, ApplicantCvGenderData)
          Dim sql = "SELECT * FROM tbl_CvGender ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvGenderData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvGenderDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvGenderDictionary
      End Get
    End Property

    Private ReadOnly Property CvHighestEducationLevelDictionary As Dictionary(Of String, ApplicantCvHighestEducationLevelData)
      Get
        If s_cvHighestEducationLevelDictionary Is Nothing Then
          s_cvHighestEducationLevelDictionary = New Dictionary(Of String, ApplicantCvHighestEducationLevelData)
          Dim sql = "SELECT * FROM tbl_CvHighestEducationLevel ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvHighestEducationLevelData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvHighestEducationLevelDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvHighestEducationLevelDictionary
      End Get
    End Property

		Private ReadOnly Property CvInstituteTypeDictionary As Dictionary(Of String, ApplicantCvInstituteTypeData)
			Get
				If s_cvInstituteTypeDictionary Is Nothing Then
					s_cvInstituteTypeDictionary = New Dictionary(Of String, ApplicantCvInstituteTypeData)
					Dim sql = "SELECT * FROM tbl_CvInstituteType ORDER BY ID ASC"
					Dim listOfParams As New List(Of SqlClient.SqlParameter)
					Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
					Try
						If (Not reader Is Nothing) Then
							While reader.Read()
								Dim data As New ApplicantCvInstituteTypeData
								data.ID = SafeGetInteger(reader, "ID", 0)
								data.Name = SafeGetString(reader, "Name", String.Empty)
								data.Code = SafeGetString(reader, "Code", String.Empty)
								data.Description = SafeGetString(reader, "Description", String.Empty)
								s_cvInstituteTypeDictionary.Add(data.Code, data)
							End While
						End If
					Catch e As Exception
						m_Logger.LogError(e.ToString())
					Finally
						CloseReader(reader)
					End Try
				End If
				Return s_cvInstituteTypeDictionary
			End Get
		End Property

    Private ReadOnly Property CvJobTitleDictionary As Dictionary(Of String, ApplicantCvJobTitleData)
      Get
        If s_cvJobTitleDictionary Is Nothing Then
          s_cvJobTitleDictionary = New Dictionary(Of String, ApplicantCvJobTitleData)
          Dim sql = "SELECT * FROM tbl_CvJobTitle ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvJobTitleData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvJobTitleDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvJobTitleDictionary
      End Get
    End Property

    Private ReadOnly Property CvLanguageSkillTypeDictionary As Dictionary(Of String, ApplicantCvLanguageSkillTypeData)
      Get
        If s_cvLanguageSkillTypeDictionary Is Nothing Then
          s_cvLanguageSkillTypeDictionary = New Dictionary(Of String, ApplicantCvLanguageSkillTypeData)
          Dim sql = "SELECT * FROM tbl_CvLanguageSkillType ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvLanguageSkillTypeData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvLanguageSkillTypeDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvLanguageSkillTypeDictionary
      End Get
    End Property

		Private ReadOnly Property CvLanguageSkillTypeList As List(Of ApplicantCvLanguageSkillTypeData)
			Get
				If s_cvLanguageSkillTypeList Is Nothing Then
					s_cvLanguageSkillTypeList = New List(Of ApplicantCvLanguageSkillTypeData)
					Dim sql = "SELECT * FROM tbl_CvLanguageSkillType ORDER BY ID ASC"
					Dim listOfParams As New List(Of SqlClient.SqlParameter)
					Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
					Try
						If (Not reader Is Nothing) Then
							While reader.Read()
								Dim data As New ApplicantCvLanguageSkillTypeData
								data.ID = SafeGetInteger(reader, "ID", 0)
								data.Name = SafeGetString(reader, "Name", String.Empty)
								data.Code = SafeGetString(reader, "Code", String.Empty)
								data.Description = SafeGetString(reader, "Description", String.Empty)
								s_cvLanguageSkillTypeList.Add(data)
							End While
						End If
					Catch e As Exception
						m_Logger.LogError(e.ToString())
					Finally
						CloseReader(reader)
					End Try
				End If
				Return s_cvLanguageSkillTypeList
			End Get
		End Property

		Private ReadOnly Property CvLanguageProficiencyDictionary As Dictionary(Of String, ApplicantCvLanguageProficiencyData)
      Get
        If s_cvLanguageProficiencyDictionary Is Nothing Then
          s_cvLanguageProficiencyDictionary = New Dictionary(Of String, ApplicantCvLanguageProficiencyData)
          Dim sql = "SELECT * FROM tbl_CvLanguageProficiency ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvLanguageProficiencyData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvLanguageProficiencyDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvLanguageProficiencyDictionary
      End Get
    End Property

    Private ReadOnly Property CvMaritalStatusDictionary As Dictionary(Of String, ApplicantCvMaritalStatusData)
      Get
        If s_cvMaritalStatusDictionary Is Nothing Then
          s_cvMaritalStatusDictionary = New Dictionary(Of String, ApplicantCvMaritalStatusData)
          Dim sql = "SELECT * FROM tbl_CvMaritalStatus ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvMaritalStatusData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvMaritalStatusDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvMaritalStatusDictionary
      End Get
    End Property

    Private ReadOnly Property CvNationalityDictionary As Dictionary(Of String, ApplicantCvNationalityData)
      Get
        If s_cvNationalityDictionary Is Nothing Then
          s_cvNationalityDictionary = New Dictionary(Of String, ApplicantCvNationalityData)
          Dim sql = "SELECT * FROM tbl_CvNationality ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvNationalityData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvNationalityDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvNationalityDictionary
      End Get
    End Property

    Private ReadOnly Property CvPhoneNumberTypeDictionary As Dictionary(Of String, ApplicantCvPhoneNumberTypeData)
      Get
        If s_cvPhoneNumberTypeDictionary Is Nothing Then
          s_cvPhoneNumberTypeDictionary = New Dictionary(Of String, ApplicantCvPhoneNumberTypeData)
          Dim sql = "SELECT * FROM tbl_CvPhoneNumberType ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvPhoneNumberTypeData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvPhoneNumberTypeDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvPhoneNumberTypeDictionary
      End Get
    End Property

    Private ReadOnly Property CvProfileStatusDictionary As Dictionary(Of String, ApplicantCvProfileStatusData)
      Get
        If s_cvProfileStatusDictionary Is Nothing Then
          s_cvProfileStatusDictionary = New Dictionary(Of String, ApplicantCvProfileStatusData)
          Dim sql = "SELECT * FROM tbl_CvProfileStatus ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvProfileStatusData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvProfileStatusDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvProfileStatusDictionary
      End Get
    End Property

    Private ReadOnly Property CvRegionDictionary As Dictionary(Of String, ApplicantCvRegionData)
      Get
        If s_cvRegionDictionary Is Nothing Then
          s_cvRegionDictionary = New Dictionary(Of String, ApplicantCvRegionData)
          Dim sql = "SELECT * FROM tbl_CvRegion ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvRegionData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvRegionDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvRegionDictionary
      End Get
    End Property

    Private ReadOnly Property CvSalaryDictionary As Dictionary(Of String, ApplicantCvSalaryData)
      Get
        If s_cvSalaryDictionary Is Nothing Then
          s_cvSalaryDictionary = New Dictionary(Of String, ApplicantCvSalaryData)
          Dim sql = "SELECT * FROM tbl_CvSalary ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvSalaryData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvSalaryDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvSalaryDictionary
      End Get
    End Property

    Private ReadOnly Property CvSocialMediaTypeDictionary As Dictionary(Of String, ApplicantCvSocialMediaTypeData)
      Get
        If s_cvSocialMediaTypeDictionary Is Nothing Then
          s_cvSocialMediaTypeDictionary = New Dictionary(Of String, ApplicantCvSocialMediaTypeData)
          Dim sql = "SELECT * FROM tbl_CvSocialMediaType ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvSocialMediaTypeData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvSocialMediaTypeDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvSocialMediaTypeDictionary
      End Get
    End Property

    Private ReadOnly Property CvSoftSkillTypeDictionary As Dictionary(Of String, ApplicantCvSoftSkillTypeData)
      Get
        If s_cvSoftSkillTypeDictionary Is Nothing Then
          s_cvSoftSkillTypeDictionary = New Dictionary(Of String, ApplicantCvSoftSkillTypeData)
          Dim sql = "SELECT * FROM tbl_CvSoftSkillType ORDER BY ID ASC"
          Dim listOfParams As New List(Of SqlClient.SqlParameter)
          Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
          Try
            If (Not reader Is Nothing) Then
              While reader.Read()
                Dim data As New ApplicantCvSoftSkillTypeData
                data.ID = SafeGetInteger(reader, "ID", 0)
                data.Name = SafeGetString(reader, "Name", String.Empty)
                data.Code = SafeGetString(reader, "Code", String.Empty)
                data.Description = SafeGetString(reader, "Description", String.Empty)
                s_cvSoftSkillTypeDictionary.Add(data.Code, data)
              End While
            End If
          Catch e As Exception
            m_Logger.LogError(e.ToString())
          Finally
            CloseReader(reader)
          End Try
        End If
        Return s_cvSoftSkillTypeDictionary
      End Get
    End Property

#End Region


#Region "Private Shared Fields"

    Private Shared s_cvAvailabilityDictionary As Dictionary(Of String, ApplicantCvAvailabilityData)
    Private Shared s_cvApprovalDictionary As Dictionary(Of String, ApplicantCvApprovalData)
    Private Shared s_cvCountryDictionary As Dictionary(Of String, ApplicantCvCountryData)
    Private Shared s_cvComputerSkillTypeDictionary As Dictionary(Of String, ApplicantCvComputerSkillTypeData)
    Private Shared s_cvDegreeDirectionDictionary As Dictionary(Of String, ApplicantCvDegreeDirectionData)
    Private Shared s_cvDiplomaDictionary As Dictionary(Of String, ApplicantCvDiplomaData)
    Private Shared s_cvDriversLicenceDictionary As Dictionary(Of String, ApplicantCvDriversLicenceData)
    Private Shared s_cvEducationDictionary As Dictionary(Of String, ApplicantCvEducationData)
    Private Shared s_cvEducationDetailDictionary As Dictionary(Of String, ApplicantCvEducationDetailData)
    Private Shared s_cvEducationLevelDictionary As Dictionary(Of String, ApplicantCvEducationLevelData)
    Private Shared s_cvEmailTypeDictionary As Dictionary(Of String, ApplicantCvEmailTypeData)
    Private Shared s_cvGenderDictionary As Dictionary(Of String, ApplicantCvGenderData)
    Private Shared s_cvHighestEducationLevelDictionary As Dictionary(Of String, ApplicantCvHighestEducationLevelData)
    Private Shared s_cvInstituteTypeDictionary As Dictionary(Of String, ApplicantCvInstituteTypeData)
    Private Shared s_cvJobTitleDictionary As Dictionary(Of String, ApplicantCvJobTitleData)
    Private Shared s_cvLanguageSkillTypeDictionary As Dictionary(Of String, ApplicantCvLanguageSkillTypeData)
		Private Shared s_cvLanguageSkillTypeList As List(Of ApplicantCvLanguageSkillTypeData)
		Private Shared s_cvLanguageProficiencyDictionary As Dictionary(Of String, ApplicantCvLanguageProficiencyData)
		Private Shared s_cvMaritalStatusDictionary As Dictionary(Of String, ApplicantCvMaritalStatusData)
    Private Shared s_cvNationalityDictionary As Dictionary(Of String, ApplicantCvNationalityData)
    Private Shared s_cvSalaryDictionary As Dictionary(Of String, ApplicantCvSalaryData)
    Private Shared s_cvPhoneNumberTypeDictionary As Dictionary(Of String, ApplicantCvPhoneNumberTypeData)
    Private Shared s_cvProfileStatusDictionary As Dictionary(Of String, ApplicantCvProfileStatusData)
    Private Shared s_cvRegionDictionary As Dictionary(Of String, ApplicantCvRegionData)
    Private Shared s_cvSocialMediaTypeDictionary As Dictionary(Of String, ApplicantCvSocialMediaTypeData)
    Private Shared s_cvSoftSkillTypeDictionary As Dictionary(Of String, ApplicantCvSoftSkillTypeData)

#End Region


  End Class


End Namespace
