
Namespace CVLizer.DataObjects


#Region "viewing data"

	Public Class CVLizerProfileViewData
		Public Property ProfileID As Integer?
		Public Property PersonalID As Integer?
		Public Property FirstName As String
		Public Property LastName As String
		Public Property WorkID As Integer?
		Public Property EducationID As Integer?
		Public Property AdditionalID As Integer?
		Public Property ObjectiveID As Integer?

		Public Property Customer_ID As String
		Public Property CreatedOn As Date?
		Public Property CreatedFrom As String
		Public Property ChangedOn As Date?
		Public Property ChangedFrom As String

		Public ReadOnly Property Fullname As String
			Get
				Return String.Format("{0} {1}", FirstName, LastName)
			End Get
		End Property

	End Class

	Public Class PersonalViewData

		Public Property PersonalID As Integer?
		Public Property FirstName As String
		Public Property LastName As String
		Public Property Gender As String
		Public Property GenderLabel As String
		Public Property Title As List(Of String)
		Public Property IsCed As String
		Public Property IsCedLable As String
		Public Property DateOfBirth As Date?
		Public Property DateOfBirthPlace As String
		Public Property Nationality As String
		Public Property NationalityLable As String
		Public Property CivilState As String
		Public Property CivilStateLable As String

		Public Property Email As List(Of String)
		Public Property PhoneNumbers As List(Of String)
		Public Property Homepage As List(Of String)
		Public Property TelefaxNumber As List(Of String)
		Public Property PersonalPhoto As Byte()
		Public Property ISValid As Boolean?


		Public ReadOnly Property Fullname As String
			Get
				Return String.Format("{0} {1}", FirstName, LastName)
			End Get
		End Property


		Public ReadOnly Property DateOfBirthYear As Integer?
			Get
				Return Year(DateOfBirth)
			End Get
		End Property


	End Class


	Public Class AddressViewData
		Public Property ID As Integer?
		Public Property Street As String
		Public Property Postcode As String
		Public Property City As String
		Public Property Country As String
		Public Property CountryLable As String
		Public Property State As String

		Public ReadOnly Property AddressLable As String
			Get
				Return String.Format("{0} {1}-{2} {3}", Street, Country, Postcode, City)
			End Get
		End Property

	End Class


	Public Class WorkPhaseViewData
		Inherits PhaseViewData

		Public Property ID As Integer?
		Public Property WorkID As Integer?
		Public Property WorkPhaseID As Integer?

		Public Property Companies As List(Of CodeViewData)
		Public Property Functions As List(Of CodeViewData)
		Public Property Positions As List(Of CodeNameViewData)
		Public Property Project As Boolean?
		Public Property Employments As List(Of CodeNameViewData)
		Public Property WorkTimes As List(Of CodeNameViewData)


#Region "readonly properties"

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

	Public Class PhaseViewData

		Public Property PhaseID As Integer?
		Public Property DateFrom As Date?
		Public Property DateTo As Date?
		Public Property DateFromFuzzy As String
		Public Property DateToFuzzy As String
		Public Property Duration As Integer?
		Public Property Current As Boolean?
		Public Property SubPhase As Boolean?
		Public Property Comments As String
		Public Property PlainText As String
		Public Property Locations As List(Of AddressViewData)
		Public Property Skills As List(Of CodeNameWeightViewData)
		Public Property SoftSkills As List(Of CodeNameWeightViewData)
		Public Property OperationAreas As List(Of CodeNameWeightViewData)
		Public Property Industries As List(Of CodeNameWeightViewData)
		Public Property CustomCodes As List(Of CodeNameWeightViewData)
		Public Property Topic As List(Of CodeViewData)
		Public Property InternetResources As List(Of InternetResourceViewData)
		Public Property DocumentID As List(Of IDiewData)


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
						value &= String.Format(" - {0}", DateViewData(DateTo)) ' String.Format(" bis {0:d}", DateTo)
					End If
				End If
				value = String.Format("{0} {1}", value, CurrentViewData)

				Return value
			End Get
		End Property

		Private ReadOnly Property DateViewData(ByVal dt As Date?) As String
			Get
				Dim value As String = String.Empty
				If Not dt.HasValue Then Return value
				If Day(dt) = 1 Then
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


#End Region


	End Class

	Public Class EducationPhaseViewData
		Inherits PhaseViewData

		Public Property ID As Integer?
		Public Property EducationID As Integer?
		Public Property EducationPhaseID As Integer?
		Public Property SchooolNames As List(Of CodeViewData)
		Public Property Graduations As List(Of CodeViewData)
		Public Property EducationTypes As List(Of CodeNameWeightViewData)
		Public Property Completed As Boolean?
		Public Property Score As Integer?
		Public Property IsCedCode As String
		Public Property IsCedCodeLable As String


#Region "readonly properties"

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



	Public Class PublicationViewData
		Inherits PhaseViewData

		Public Property ID As Integer?
		Public Property Author As List(Of CodeViewData)
		Public Property Proceedings As String
		Public Property Institute As String


		Public ReadOnly Property AuthorLable As String
			Get
				Dim value As String = String.Empty
				If Not Author Is Nothing AndAlso Author.Count > 0 Then
					For Each itm In Author
						value &= If(String.IsNullOrWhiteSpace(value), String.Empty, vbNewLine) & itm.Lable
					Next
				End If

				Return value
			End Get
		End Property


	End Class


	Public Class AdditionalInfoViewData

		Public Property ID As Integer?
		Public Property Languages As List(Of LanguageData)
		Public Property DrivingLicences As List(Of CodeViewData)
		Public Property MilitaryService As Boolean?
		Public Property Competences As String
		Public Property Interests As String
		Public Property Additionals As String
		Public Property UndatedSkills As List(Of CodeNameWeightViewData)
		Public Property UndatedOperationArea As List(Of CodeNameWeightViewData)
		Public Property UndatedIndustries As List(Of CodeNameWeightViewData)
		Public Property InternetResources As List(Of InternetResourceViewData)


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

		Public ReadOnly Property LanguageViewData As IEnumerable(Of CodeNameViewData)
			Get
				If Languages Is Nothing OrElse Languages.Count = 0 Then Return Nothing
				Dim value As List(Of CodeNameViewData) = Nothing
				For Each itm In Languages
					Dim data = New CodeNameViewData
					data.Code = itm.CodeName
					data.Name = itm.Level.CodeName

					value.Add(data)
				Next

				Return value
			End Get
		End Property


#End Region


	End Class


	Public Class DocumentViewData

		Public Property ID As Integer?
		Public Property DocClass As String
		Public Property Pages As Integer?
		Public Property Plaintext As String
		Public Property FileType As String
		Public Property DocBinary As Byte()
		Public Property DocID As Integer?
		Public Property DocSize As Integer?
		Public Property DocLanguage As String
		Public Property FileHashvalue As String
		Public Property DocXML As String


	End Class




	Public Class PersonalListViewData
		Public Property ID As Integer?
		Public Property PersonalID As Integer?
		Public Property Lable As String

	End Class

	Public Class CodeViewData
		Public Property ID As Integer?
		Public Property Lable As String

	End Class

	Public Class IDiewData
		Public Property ID As Integer?
		Public Property CodeNumber As Integer

	End Class

	Public Class CodeNameWeightViewData
		Public Property ID As Integer?
		Public Property PhaseID As Integer?
		Public Property Code As String
		Public Property Name As String
		Public Property Weight As Double?

	End Class

	Public Class CodeNameViewData
		Public Property ID As Integer?
		Public Property PhaseID As Integer?
		Public Property Code As String
		Public Property Name As String

	End Class

	Public Class InternetResourceViewData
		Public Property ID As Integer?
		Public Property PhaseID As Integer?
		Public Property URL As String
		Public Property Title As String
		Public Property Source As String
		Public Property Snippet As String

	End Class



#End Region


#Region "search data"

	Public Class CVLExperiencesData

		Public Property ID As Integer?
		Public Property ProfileID As Integer?
		Public Property CustomerID As String
		Public Property Code As String
		Public Property CodeLabel As String
		Public Property DateFrom As DateTime?
		Public Property DateTo As DateTime?
		Public Property Duration As Integer?
		Public Property ExperiencesKind As ExperiencesEnum

	End Class

	Public Enum ExperiencesEnum
		SKILLS
		OPERATIONAREA
		JOBFUNCTIONS
		LANGUAGES
	End Enum

#End Region

End Namespace

