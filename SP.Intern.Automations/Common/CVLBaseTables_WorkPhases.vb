
Imports System.ComponentModel
Imports SP.Internal.Automations.SPApplicationWebService


Namespace BaseTable

  Partial Class SPSBaseTables


#Region "public properties"

    Public Property CVLPersonalID As Integer?
    Public Property CVLWorkID As Integer?
    Public Property CVLEducationID As Integer?
    Public Property CVLAdditionalID As Integer?
    Public Property CVLObjectiveID As Integer?

    Public ReadOnly Property AdditionalInfoData As AdditionalInfoLocalViewData
      Get
        Return m_AdditionalInfoData
      End Get
    End Property

    Public ReadOnly Property AdditionalLanguageData As BindingList(Of CodeNameViewData)
      Get
        Return m_LanguageData
      End Get
    End Property


#End Region


#Region "public methodes"

    Public Function LoadCVLProfileData(ByVal profileID As Integer?) As Boolean
      Dim result As Boolean = True

      result = result AndAlso PerformCVLProfileDataWebservice(profileID)


      Return result

    End Function

    Public Function LoadCVLWorkPhases(ByVal profileID As Integer?, ByVal workID As Integer?) As BindingList(Of WorkPhaseLocalViewData)
      Dim listDataSource As BindingList(Of WorkPhaseLocalViewData) = New BindingList(Of WorkPhaseLocalViewData)

      If workID.GetValueOrDefault(0) = 0 Then
        Dim success As Boolean = PerformCVLProfileDataWebservice(profileID)
        If Not success Then Return Nothing

        workID = m_WorkID
      End If
      listDataSource = PerformCVLWorkPhaseWebservice(profileID, workID)


      Return listDataSource

    End Function

    Public Function LoadCVLEducationData(ByVal profileID As Integer?, ByVal cvlEducationID As Integer?) As BindingList(Of EducationPhaseLocalViewData)
      Dim listDataSource As BindingList(Of EducationPhaseLocalViewData) = New BindingList(Of EducationPhaseLocalViewData)

      If cvlEducationID.GetValueOrDefault(0) = 0 Then
        Dim success As Boolean = PerformCVLProfileDataWebservice(profileID)
        If Not success Then Return Nothing

        cvlEducationID = m_EducationID
      End If
      listDataSource = PerformEducationPhaseWebservice(profileID, cvlEducationID)


      Return listDataSource

    End Function

    Public Function LoadCVLAdditionalInfos(ByVal profileID As Integer?, ByVal personalID As Integer?) As Boolean
      Dim result As Boolean = True

      If personalID.GetValueOrDefault(0) = 0 Then
        result = result AndAlso PerformCVLProfileDataWebservice(profileID)

        personalID = m_PersonalID
      End If
      result = result AndAlso PerformAdditionalDataWebservice(profileID, personalID)


      Return result

    End Function


#End Region


#Region "private methodes"

    Private Function PerformCVLProfileDataWebservice(ByVal profileID As Integer?) As Boolean
      If profileID Is Nothing Then Return False

			Dim ws = New SPApplicationWebService.SPApplicationSoapClient
      ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

      ' Read data over webservice
      Dim searchResult = ws.LoadAssignedCVLProfileViewData(String.Empty, profileID)
      If searchResult Is Nothing Then
        m_Logger.LogError(String.Format("profileData: could not be loaded from webservice! {0} | {1}", m_customerID, profileID))

        Return False
      End If

      m_WorkID = searchResult.WorkID
      m_PersonalID = searchResult.PersonalID
      m_EducationID = searchResult.EducationID
      m_AdditionalID = searchResult.AdditionalID
      m_ObjectiveID = searchResult.ObjectiveID

      CVLWorkID = searchResult.WorkID
      CVLPersonalID = searchResult.PersonalID
      CVLEducationID = searchResult.EducationID
      CVLAdditionalID = searchResult.AdditionalID
      CVLObjectiveID = searchResult.ObjectiveID

      Return Not (searchResult Is Nothing)

    End Function

    Private Function PerformCVLWorkPhaseWebservice(ByVal profileID As Integer?, ByVal workID As Integer?) As BindingList(Of WorkPhaseLocalViewData)
      Dim listDataSource As BindingList(Of WorkPhaseLocalViewData) = New BindingList(Of WorkPhaseLocalViewData)


			Dim ws = New SPApplicationWebService.SPApplicationSoapClient
      ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

      Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

      ' Read data over webservice
      Dim searchResult = ws.LoadCVLWorkPhaseViewData(m_customerID, profileID, workID)
      If searchResult Is Nothing Then
        m_Logger.LogError(String.Format("LoadCVLWorkPhaseViewData: could not be loaded from webservice! {0} | {1} | {2}", m_customerID, profileID, workID))

        Return Nothing
      End If

      Dim gridData = (From person In searchResult
                      Select New WorkPhaseLocalViewData With {.ID = person.ID,
                        .WorkPhaseID = person.WorkPhaseID,
                        .PhaseID = person.PhaseID,
                        .Project = person.Project,
                        .DateFrom = person.DateFrom,
                        .Companies = person.Companies,
                        .DateTo = person.DateTo,
                        .DateFromFuzzy = person.DateFromFuzzy,
                        .DateToFuzzy = person.DateToFuzzy,
                        .Duration = person.Duration,
                        .Current = person.Current,
                        .SubPhase = person.SubPhase,
                        .Comments = person.Comments,
                        .PlainText = person.PlainText,
                        .Functions = person.Functions,
                        .Positions = person.Positions,
                        .Employments = person.Employments,
                        .WorkTimes = person.WorkTimes,
                        .Locations = person.Locations,
                        .Skills = person.Skills,
                        .SoftSkills = person.SoftSkills,
                        .OperationAreas = person.OperationAreas,
                        .Industries = person.Industries,
                        .CustomCodes = person.CustomCodes,
                        .Topic = person.Topic,
                        .InternetResources = person.InternetResources,
                        .DocumentID = person.DocumentID
                        }).ToList()

      For Each p In gridData
        listDataSource.Add(p)
      Next


      Return listDataSource

    End Function

    Private Function PerformEducationPhaseWebservice(ByVal profileID As Integer?, ByVal cvlEducationID As Integer?) As BindingList(Of EducationPhaseLocalViewData)
      Dim listDataSource As BindingList(Of EducationPhaseLocalViewData) = New BindingList(Of EducationPhaseLocalViewData)

			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

      Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

      ' Read data over webservice
      Dim searchResult = webservice.LoadCVLEducationPhaseViewData(m_customerID, profileID, cvlEducationID)
      If searchResult Is Nothing Then
        m_Logger.LogError("LoadCVLEducationPhaseViewData: value is nothing!")

        Return Nothing
      End If

      Dim gridData = (From person In searchResult
                      Select New EducationPhaseLocalViewData With {.ID = person.ID,
                      .EducationID = person.EducationID,
                      .PhaseID = person.PhaseID,
                      .EducationPhaseID = person.EducationPhaseID,
                      .SchooolNames = person.SchooolNames,
                      .Graduations = person.Graduations,
                      .EducationTypes = person.EducationTypes,
                      .Completed = person.Completed,
                      .Score = person.Score,
                      .IsCedCode = person.IsCedCode,
                      .IsCedCodeLable = person.IsCedCodeLable,
                      .DateFrom = person.DateFrom,
                      .DateTo = person.DateTo,
                      .DateFromFuzzy = person.DateFromFuzzy,
                      .DateToFuzzy = person.DateToFuzzy,
                      .Duration = person.Duration,
                      .Current = person.Current,
                      .SubPhase = person.SubPhase,
                      .Comments = person.Comments,
                      .PlainText = person.PlainText,
                      .Locations = person.Locations,
                      .Skills = person.Skills,
                      .SoftSkills = person.SoftSkills,
                      .OperationAreas = person.OperationAreas,
                      .Industries = person.Industries,
                      .CustomCodes = person.CustomCodes,
                      .Topic = person.Topic,
                      .InternetResources = person.InternetResources,
                      .DocumentID = person.DocumentID
                      }).ToList()

      For Each p In gridData
        listDataSource.Add(p)
      Next


      Return listDataSource

    End Function

    Private Function PerformAdditionalDataWebservice(ByVal profileID As Integer?, ByVal personalID As Integer?) As Boolean
      Dim result As Boolean = True
			'Dim additionalData As AdditionalInfoLocalViewData


			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
      webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

      Trace.WriteLine(String.Format("LoadCVLAdditionalInfoViewData: Customer_ID: {0} contacting...", m_customerID))

      ' Read data over webservice
      Dim searchResult = webservice.LoadCVLAdditionalInfoViewData(m_customerID, profileID, personalID)
      If searchResult Is Nothing Then
        m_Logger.LogError(String.Format("additional informations could not be loaded from webservice! {0} | {1} | {2}", m_customerID, profileID, personalID))

        Return Nothing
      End If

			m_AdditionalInfoData = (New AdditionalInfoLocalViewData With {.Additionals = searchResult.Additionals,
              .Competences = searchResult.Competences,
              .DrivingLicences = searchResult.DrivingLicences, .ID = searchResult.ID,
              .Interests = searchResult.Interests, .InternetResources = searchResult.InternetResources,
              .Languages = searchResult.Languages,
              .MilitaryService = searchResult.MilitaryService,
              .UndatedIndustries = searchResult.UndatedIndustries,
              .UndatedOperationArea = searchResult.UndatedOperationArea,
              .UndatedSkills = searchResult.UndatedSkills
            })


			If Not m_AdditionalInfoData.Languages Is Nothing Then
				m_LanguageData = New BindingList(Of CodeNameViewData)

				For Each language In m_AdditionalInfoData.Languages
					Dim lang = New CodeNameViewData
					lang.Code = language.CodeName
					lang.Name = language.Level.CodeName

					m_LanguageData.Add(lang) 'New CodeNameViewData With {.Code = language.CodeName, .Name = language.Level.CodeName})
				Next
			End If


			Return Not (m_LanguageData Is Nothing)

    End Function


#End Region



  End Class



End Namespace


Public Class WorkPhaseLocalViewData
    Inherits WorkPhaseViewDataDTO

#Region "readonly properties"

    Public ReadOnly Property DateFromToViewData As String
      Get
        Dim value As String = String.Empty
        If DateFrom.HasValue Then
          If DateFrom = CDate("01.01." & Year(DateFrom)) Then
            value = String.Format("{0:F0}", Year(DateFrom))
          Else
            value = String.Format("{0}", DateViewData(DateFrom))
          End If

        End If
        If DateTo.HasValue Then
          If DateTo = CDate("01.01." & Year(DateTo)) Then
            value &= String.Format(" - {0:F0}", Year(DateTo))
          Else
            value &= String.Format(" - {0}", DateViewData(DateTo))
          End If
        End If
        value = String.Format("{0} {1}", value, CurrentViewData)

        Return value
      End Get
    End Property

    Public ReadOnly Property DateViewData(ByVal dt As Date?) As String
      Get
        Dim value As String = String.Empty
        If Not dt.HasValue Then Return value
        If DateAndTime.Day(dt) = 1 Then
          value = String.Format("{0:MM.yyyy}", dt)
        Else
          value = String.Format("{0:d}", dt)
        End If

        Return value
      End Get
    End Property

    Public ReadOnly Property DurationViewData As String
      Get
        Dim value As String = String.Empty
        If Duration.GetValueOrDefault(0) = 0 Then Return value

        Dim yrs As Integer
        Dim mos As Integer

        yrs = Math.DivRem(Duration.GetValueOrDefault(0), 12, mos)
        If yrs > 0 Then
          If yrs = 1 Then
            value = String.Format("{0:F0} Monat{1}", Duration.GetValueOrDefault(0), If(Duration.GetValueOrDefault(0) > 1, "e", ""))

            Return value
          Else
            value = String.Format("{0:F0} Jahr{1}", yrs, If(yrs > 1, "e", ""))
          End If

        End If

        If mos > 0 Then
          value = String.Format("{0}{1}{2:F0} Monat{3}", value, If(yrs > 0, " und ", ""), mos, If(mos > 1, "e", ""))
        End If


        Return If(Duration.GetValueOrDefault(0) > 0, value, String.Empty)
      End Get
    End Property

    Public ReadOnly Property CommentsLable As String
      Get
        Dim value As String = String.Empty
        If Not Comments Is Nothing Then
          value = Comments.Replace(vbNewLine, " ")
        End If

        Return value
      End Get
    End Property

    Public ReadOnly Property PlainTextLable As String
      Get
        Dim value As String = String.Empty
        If Not PlainText Is Nothing Then
          value = PlainText.Replace(vbNewLine, " ").Replace(vbLf, " ")
        End If

        Return value
      End Get
    End Property

    Public ReadOnly Property LocationViewData As String
      Get
        If Locations Is Nothing OrElse Locations.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In Locations
          value &= String.Format("{0}{1}{2}{3}", If(String.IsNullOrWhiteSpace(value), "", vbNewLine), itm.City, If(String.IsNullOrWhiteSpace(itm.CountryLable), "", " - "), itm.CountryLable)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property SkillViewData As String
      Get
        If Skills Is Nothing OrElse Skills.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In Skills
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property SoftSkillViewData As String
      Get
        If SoftSkills Is Nothing OrElse SoftSkills.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In SoftSkills
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property OperationAreasViewData As String
      Get
        If OperationAreas Is Nothing OrElse OperationAreas.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In OperationAreas
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property IndustryViewData As String
      Get
        If Industries Is Nothing OrElse Industries.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In Industries
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property CustomCodesViewData As String
      Get
        If CustomCodes Is Nothing OrElse CustomCodes.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In CustomCodes
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property TopicViewData As String
      Get
        If Topic Is Nothing OrElse Topic.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In Topic
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property InternetResourcesViewData As String
      Get
        If InternetResources Is Nothing OrElse InternetResources.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In InternetResources
          value &= String.Format("{0}{1}{2}", If(String.IsNullOrWhiteSpace(value), "", vbNewLine), itm.Title, itm.URL)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property DocumentIDViewData As String
      Get
        If DocumentID Is Nothing OrElse DocumentID.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In DocumentID
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.CodeNumber)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property CurrentViewData As String
      Get
        Dim value As String = "(derzeit)"
        If Not Current.GetValueOrDefault(False) Then value = ""

        Return value
      End Get
    End Property



    Public ReadOnly Property CompanyViewData As String
      Get
        If Companies Is Nothing OrElse Companies.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In Companies
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property FunctionViewData As String
      Get
        If Functions Is Nothing OrElse Functions.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In Functions
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property PositionViewData As String
      Get
        If Positions Is Nothing OrElse Positions.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In Positions
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property EmploymentViewData As String
      Get
        If Employments Is Nothing OrElse Employments.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In Employments
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property WorkTimeViewData As String
      Get
        If WorkTimes Is Nothing OrElse WorkTimes.Count = 0 Then Return Nothing
        Dim value As String = String.Empty
        For Each itm In WorkTimes
          value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
        Next

        Return value
      End Get
    End Property

    Public ReadOnly Property ProjectViewData As String
      Get
        Dim value As String = "Nein"
        If Not Project.GetValueOrDefault(False) Then value = "Nein" Else value = "Ja"

        Return value
      End Get
    End Property


#End Region


  End Class


Public Class AdditionalInfoLocalViewData
  Inherits AdditionalInfoViewDataDTO


#Region "readonly properties"

  Public ReadOnly Property MilitaryServiceViewData As String
    Get
      Dim value As String = "Ja"
      If Not MilitaryService.GetValueOrDefault(False) Then value = "Nein"

      Return value
    End Get
  End Property

  Public ReadOnly Property DrivingLicenceViewData As String
    Get
      If DrivingLicences Is Nothing OrElse DrivingLicences.Count = 0 Then Return Nothing
      Dim value As String = String.Empty
      For Each itm In DrivingLicences
        value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
      Next

      Return value
    End Get
  End Property

  Public ReadOnly Property UndatedSkillViewData As String
    Get
      If UndatedSkills Is Nothing OrElse UndatedSkills.Count = 0 Then Return Nothing
      Dim value As String = String.Empty
      For Each itm In UndatedSkills
        value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
      Next

      Return value
    End Get
  End Property

  Public ReadOnly Property UndatedOperationAreViewData As String
    Get
      If UndatedOperationArea Is Nothing OrElse UndatedOperationArea.Count = 0 Then Return Nothing
      Dim value As String = String.Empty
      For Each itm In UndatedOperationArea
        value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
      Next

      Return value
    End Get
  End Property

  Public ReadOnly Property UndatedIndustryViewData As String
    Get
      If UndatedIndustries Is Nothing OrElse UndatedIndustries.Count = 0 Then Return Nothing
      Dim value As String = String.Empty
      For Each itm In UndatedIndustries
        value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
      Next

      Return value
    End Get
  End Property

  Public ReadOnly Property InternetResourcesViewData As String
    Get
      If InternetResources Is Nothing OrElse InternetResources.Count = 0 Then Return Nothing
      Dim value As String = String.Empty
      For Each itm In InternetResources
        value &= String.Format("{0}{1}{2}", If(String.IsNullOrWhiteSpace(value), "", vbNewLine), itm.Title, itm.URL)
      Next

      Return value
    End Get
  End Property

  Public ReadOnly Property LanguageViewData As IEnumerable(Of DatabaseAccess.CVLizer.DataObjects.CodeNameViewData)
    Get
      If Languages Is Nothing OrElse Languages.Count = 0 Then Return Nothing
      Dim value As List(Of DatabaseAccess.CVLizer.DataObjects.CodeNameViewData) = Nothing
      For Each itm In Languages
        Dim data = New DatabaseAccess.CVLizer.DataObjects.CodeNameViewData
        data.Code = itm.CodeName
        data.Name = itm.Level.CodeName

        value.Add(data)
      Next

      Return value
    End Get
  End Property


#End Region


End Class


Public Class EducationPhaseLocalViewData
  Inherits EducationPhaseViewDataDTO


#Region "readonly properties"

  Public ReadOnly Property DateFromToViewData As String
    Get
      Dim value As String = String.Empty
      If DateFrom.HasValue Then
        If DateFrom = CDate("01.01." & Year(DateFrom)) Then
          value = String.Format("{0:F0}", Year(DateFrom))
        Else
          value = String.Format("{0}", DateViewData(DateFrom))
        End If

      End If
      If DateTo.HasValue Then
        If DateTo = CDate("01.01." & Year(DateTo)) Then
          value &= String.Format(" - {0:F0}", Year(DateTo))
        Else
          value &= String.Format(" - {0}", DateViewData(DateTo))
        End If
      End If
      value = String.Format("{0} {1}", value, CurrentViewData)

      Return value
    End Get
  End Property

  Public ReadOnly Property DateViewData(ByVal dt As Date?) As String
    Get
      Dim value As String = String.Empty
      If Not dt.HasValue Then Return value
      If DateAndTime.Day(dt) = 1 Then
        value = String.Format("{0:MM.yyyy}", dt)
      Else
        value = String.Format("{0:d}", dt)
      End If

      Return value
    End Get
  End Property

  Public ReadOnly Property DurationViewData As String
    Get
      Dim value As String = String.Empty
      If Duration.GetValueOrDefault(0) = 0 Then Return value

      Dim yrs As Integer
      Dim mos As Integer

      yrs = Math.DivRem(Duration.GetValueOrDefault(0), 12, mos)
      If yrs > 0 Then
        If yrs = 1 Then
          value = String.Format("{0:F0} Monat{1}", Duration.GetValueOrDefault(0), If(Duration.GetValueOrDefault(0) > 1, "e", ""))

          Return value
        Else
          value = String.Format("{0:F0} Jahr{1}", yrs, If(yrs > 1, "e", ""))
        End If

      End If

      If mos > 0 Then
        value = String.Format("{0}{1}{2:F0} Monat{3}", value, If(yrs > 0, " und ", ""), mos, If(mos > 1, "e", ""))
      End If


      Return If(Duration.GetValueOrDefault(0) > 0, value, String.Empty)
    End Get
  End Property

  Public ReadOnly Property CurrentViewData As String
    Get
      Dim value As String = "(derzeit)"
      If Not Current.GetValueOrDefault(False) Then value = ""

      Return value
    End Get
  End Property

  Public ReadOnly Property SchoolnameViewData As String
    Get
      If SchooolNames Is Nothing OrElse SchooolNames.Count = 0 Then Return Nothing
      Dim value As String = String.Empty
      For Each itm In SchooolNames
        value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
      Next

      Return value
    End Get
  End Property

  Public ReadOnly Property GraduationViewData As String
    Get
      If Graduations Is Nothing OrElse Graduations.Count = 0 Then Return Nothing
      Dim value As String = String.Empty
      For Each itm In Graduations
        value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
      Next

      Return value
    End Get
  End Property

  Public ReadOnly Property EducationTypeViewData As String
    Get
      If EducationTypes Is Nothing OrElse EducationTypes.Count = 0 Then Return Nothing
      Dim value As String = String.Empty
      For Each itm In EducationTypes
        value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
      Next

      Return value
    End Get
  End Property

  Public ReadOnly Property CompletedViewData As String
    Get
      Dim value As String = "Ja"
      If Not Completed.GetValueOrDefault(False) Then value = "Nein"

      Return value
    End Get
  End Property

#End Region


End Class
