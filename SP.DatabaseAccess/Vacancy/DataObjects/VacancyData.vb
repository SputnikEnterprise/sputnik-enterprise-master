

Namespace Vacancy.DataObjects

	Public Class VacancyMasterData

		Public Property ID As Integer?
		Public Property VakNr As Integer?
		Public Property Berater As String
		Public Property AdvisorNumber As Integer
		Public Property Filiale As String
		Public Property VakKontakt As String
		Public Property VakState As String
		Public Property VakKontakt_Value As Integer?
		Public Property VakState_Value As Integer?
		Public Property Bezeichnung As String
		Public Property Slogan As String
		Public Property Gruppe As String
		Public Property SubGroup As String
		Public Property KDNr As Integer?
		Public Property SBNNumber As Integer?
		Public Property SBNPublicationState As Integer?
		Public Property SBNPublicationDate As DateTime?
		Public Property SBNPublicationFrom As String
		Public Property KDZHDNr As Integer?
		Public Property ExistLink As Boolean?
		Public Property VakLink As String
		Public Property Beginn As String
		Public Property JobProzent As String
		Public Property Anstellung As String
		Public Property Dauer As String
		Public Property MAAge As String
		Public Property MASex As String
		Public Property MAZivil As String
		Public Property MALohn As String
		Public Property Jobtime As String
		Public Property JobOrt As String
		Public Property MAFSchein As String
		Public Property MAAuto As String
		Public Property MANationality As String
		Public Property IEExport As Boolean?
		Public Property JobChannelPriority As Boolean?
		Public Property IsJobsCHOnline As Boolean?
		Public Property IsOstJobOnline As Boolean?
		Public Property KDBeschreibung As String
		Public Property KDBietet As String
		Public Property SBeschreibung As String
		Public Property Reserve1 As String
		Public Property Taetigkeit As String
		Public Property Anforderung As String
		Public Property Reserve2 As String
		Public Property Reserve3 As String
		Public Property Ausbildung As String
		Public Property Weiterbildung As String
		Public Property SKennt As String
		Public Property EDVKennt As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property Result As String
		Public Property Vak_Region As String
		Public Property FirstTransferDate As DateTime?
		Public Property Transfered_User As String
		Public Property Transfered_On As DateTime?
		Public Property Transfered_Guid As String
		Public Property Vak_Kanton As String
		Public Property Customer_Guid As String
		Public Property Bemerkung As String
		Public Property MDNr As Integer?
		Public Property JobPLZ As String
		Public Property UserKontakt As String
		Public Property UserEMail As String
		Public Property TitelForSearch As String
		Public Property ShortDescription As String
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedUserNumber As Integer?

		Public Property VacancyNumberOffset As Integer

	End Class


	Public Class VacancyJobCHMasterData

		Public Property ID As Integer?
		Public Property VakNr As Integer?
		Public Property UserKontakt As String
		Public Property UserEMail As String
		Public Property TitelForSearch As String
		Public Property ShortDescription As String
		Public Property Beginn As String
		Public Property JobProzent As String
		Public Property Anstellung As String
		Public Property Dauer As String
		Public Property MASex As String
		Public Property Vak_Kanton As String
		Public Property MAAge As String
		Public Property IEExport As Boolean?
		Public Property JobChannelPriority As Boolean?
		Public Property Firma1 As String
		Public Property KDzNachname As String
		Public Property KDzVorname As String
		Public Property USVorname As String
		Public Property USNachname As String
		Public Property BranchenValue As Integer?
		Public Property BranchenBez As String
		Public Property Position_Value As Integer?
		Public Property Position As String
		Public Property Organisation_ID As Integer?
		Public Property Organisation_SubID As Integer?
		Public Property Inserat_ID As String
		Public Property Our_URL As String
		Public Property Direkt_URL As String
		Public Property Branche As String
		Public Property Vak_Sprache As String
		Public Property Layout_ID As Integer?
		Public Property Logo_ID As Integer?
		Public Property Bewerben_URL As String
		Public Property Angebot_Value As Integer?
		Public Property Xing_Poster_URL As String
		Public Property Xing_Company_Profile_URL As String
		Public Property Xing_Company_Is_Poc As Boolean?
		Public Property StartDate As DateTime?
		Public Property EndDate As DateTime?
		Public Property IsOnline As Boolean?
		Public Property FirstJobsCHTransferDate As DateTime?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property USJCHSub_ID As Integer?
		Public Property USJCHLayout_ID As Integer?
		Public Property USJCHLogo_ID As Integer?
		Public Property USJCHOur_URL As String
		Public Property DaysToAdd As Integer?
		Public Property USJCHDirekt_URL As String
		Public Property USJCHXing_Poster_URL As String
		Public Property USJCHXing_Company_Profile_URL As String
		Public Property USJCHXing_Company_Is_Poc As Boolean?
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedUserNumber As Integer?

	End Class

	Public Class VacancyJobCHBerufData
		Public Property ID As Integer?
		Public Property VakNr As Integer?
		Public Property BerufGruppe_Value As Integer?
		Public Property BerufGruppe As String
		Public Property Fachrichtung_Value As Integer?
		Public Property Fachrichtung As String
		Public Property Position_Value As Integer?
		Public Property Position As String
		Public Property ForExperience As Boolean?

	End Class

	Public Class VacancyJobCHLanguageData
		Public Property ID As Integer?
		Public Property VakNr As Integer?
		Public Property LanguageNiveau_Value As Integer?
		Public Property Bezeichnung_Value As Integer?
		Public Property LanguageNiveau As String
		Public Property Bezeichnung As String

		Public ReadOnly Property LanguageViewData
			Get
				Return String.Format(String.Format("{0}: {1}", Bezeichnung, LanguageNiveau))
			End Get
		End Property

	End Class

	Public Class VacancyJobCHPeripheryData
		Public Property ID As Integer?
		Public Property VakNr As Integer?
		Public Property Bez_Value As Integer?
		Public Property Bezeichnung As String

	End Class

	Public Class JobCHCounterData

		Public Property AllowedJobQuantity As Integer
		Public Property ExportedJobQuantity As Integer
		Public Property ExpireSoonJobQuantity As Integer
		Public Property IsCounterOK As Boolean

	End Class

	Public Class OstJobCounterData

		Public Property AllowedJobQuantity As Integer
		Public Property ExportedJobQuantity As Integer
		Public Property ExpireSoonJobQuantity As Integer
		Public Property IsCounterOK As Boolean

	End Class

	Public Class VacancyInseratData

		Public Property KDBeschreibung As String
		Public Property KDBietet As String
		Public Property SBeschreibung As String
		Public Property Reserve1 As String
		Public Property Taetigkeit As String
		Public Property Anforderung As String
		Public Property Reserve2 As String
		Public Property Reserve3 As String
		Public Property Ausbildung As String
		Public Property Weiterbildung As String
		Public Property SKennt As String
		Public Property EDVKennt As String

	End Class

	Public Class VacancyInseratJobCHData

		Public Property Bezeichnung As String
		Public Property Vorspann As String
		Public Property Aufgabe As String
		Public Property Anforderung As String
		Public Property Wirbieten As String

		Public ReadOnly Property Vorschau As String
			Get
				Return String.Format("{0}<h2>{1}</h2>{2}<br>{3}<br>{4}", Vorspann, Bezeichnung, Aufgabe, Anforderung, Wirbieten)
			End Get
		End Property

	End Class


	Public Class VacancyOstJobMasterData

		Public Property id As Integer?
		Public Property VakNr As Integer?
		Public Property UserNr As Integer?

		Public Property interneid As String
		Public Property keywords As String
		Public Property linkiframe As String
		Public Property USOSJDirekt_URL As String
		Public Property bewerberlink As String

		Public Property startdate As Date?
		Public Property enddate As Date?

		Public Property ostjob As Boolean?
		Public Property zentraljob As Boolean?
		Public Property minisite As Boolean?
		Public Property nicejob As Boolean?
		Public Property westjob As Boolean?

		Public Property companyhomepage As Boolean?
		Public Property lehrstelle As Boolean?

		Public Property layoutid As Integer?

		Public Property FirstOstJobTransferDate As DateTime?
		Public Property createdon As DateTime?
		Public Property createdfrom As String
		Public Property changedon As DateTime?
		Public Property changedfrom As String
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedUserNumber As Integer?

		Public Property isonline As Boolean?

		Public ReadOnly Property Direkt_Link As String
			Get
				If String.IsNullOrWhiteSpace(linkiframe) OrElse linkiframe Is Nothing Then
					Return USOSJDirekt_URL
				Else
					Return linkiframe
				End If
			End Get
		End Property

	End Class


	Public Class VacancyStmpSettingData

		Public Property ID As Integer?
		Public Property VakNr As Integer?
		Public Property EducationCode As Integer?
		Public Property PublicationStartDate As DateTime?
		Public Property PublicationEndDate As DateTime?
		Public Property NumberOfJobs As Integer?
		Public Property IsOnline As Boolean?
		Public Property Less_One_Year As Boolean?
		Public Property More_One_Year As Boolean?
		Public Property More_Three_Years As Boolean?
		Public Property Sunday_and_Holidays As Boolean?
		Public Property Shift_Work As Boolean?
		Public Property Night_Work As Boolean?
		Public Property Home_Work As Boolean?
		Public Property ReportToAvam As Boolean?
		Public Property ShortEmployment As Boolean?
		Public Property Immediately As Boolean?
		Public Property Surrogate As Boolean?
		Public Property Permanent As Boolean?
		Public Property EuresDisplay As Boolean?
		Public Property PublicDisplay As Boolean?

		Public Property AVAMRecordState As String
		Public Property JobroomID As String
		Public Property stellennummerEgov As String
		Public Property ReportingObligation As Boolean?
		Public Property ReportingObligationEndDate As DateTime?
		Public Property ReportingDate As DateTime?
		Public Property ReportingFrom As String
		Public Property SyncDate As DateTime?
		Public Property SyncFrom As String


		Public ReadOnly Property AVAMStateEnum As AVAMState

			Get
				Dim value As AVAMState = DataObjects.AVAMState.INSPECTING

				If String.IsNullOrWhiteSpace(AVAMRecordState) OrElse AVAMRecordState = "INSPECTING" Then value = DataObjects.AVAMState.INSPECTING
				If AVAMRecordState = "REJECTED" Then value = AVAMState.REJECTED
				If AVAMRecordState = "PUBLISHED_RESTRICTED" Then value = DataObjects.AVAMState.PUBLISHED_RESTRICTED
				If AVAMRecordState = "PUBLISHED_PUBLIC" Then value = DataObjects.AVAMState.PUBLISHED_PUBLIC
				If AVAMRecordState = "CANCELLED" Then value = DataObjects.AVAMState.CANCELLED
				If AVAMRecordState = "ARCHIVED" Then value = DataObjects.AVAMState.ARCHIVED


				Return value

			End Get

		End Property


	End Class


	Public Enum AVAMState

		INSPECTING
		REJECTED
		PUBLISHED_RESTRICTED
		PUBLISHED_PUBLIC
		CANCELLED
		ARCHIVED

	End Enum



	Public Class VacancyJobCHPlattformCustomerData

		Public Property ID As Integer?
		Public Property StartDate As DateTime?
		Public Property EndDate As DateTime?
		Public Property Jobplattform_Art As Integer?
		Public Property Logo_ID As Integer?
		Public Property Layout_ID As Integer?
		Public Property customerID As String
		Public Property DaysToAdd As Integer?
		Public Property Organisation_ID As Integer?
		Public Property Organisation_Kontingent As Integer?
		Public Property Our_URL As String
		Public Property Direkt_URL As String
		Public Property Vak_Sprache As String
		Public Property Xing_Poster_URL As String
		Public Property Xing_Company_Profile_URL As String
		Public Property Xing_Company_Is_Poc As Boolean?
		Public Property Dirctlink_iframe As String
		Public Property Bewerberform As String
		Public Property Bewerben_URL As String

	End Class

	Public Class VacancyOstJobPlattformCustomerData

		Public Property ID As Integer?
		Public Property customerID As String
		Public Property StartDate As DateTime?
		Public Property EndDate As DateTime?
		Public Property DaysToAdd As Integer?
		Public Property layoutid As Integer?
		Public Property companyhomepage As Boolean?
		Public Property lehrstelle As Boolean?
		Public Property ostjob As Boolean?
		Public Property zentraljob As Boolean?
		Public Property minisite As Boolean?
		Public Property nicejob As Boolean?
		Public Property westjob As Boolean?

	End Class

	Public Enum ExternalPlattforms
		INTERNAL
		JOBSCH
		OSTJOB
		WESTJOB
		AVAM
	End Enum

	Public Class VacancyZusatzMenuData

		Public Property ID As Integer?
		Public Property RecNr As Integer?
		Public Property GroupNr As Integer?
		Public Property Bezeichnung As String
		Public Property DBFieldName As String
		Public Property ShowInMAVersand As Boolean?
		Public Property ShowInProposeNavBar As Boolean?
		Public Property ModulName As String

	End Class

	Public Enum DeleteVacancyResult

		Deleted = 2
		ErrorWhileDelete = 4

	End Enum

End Namespace
