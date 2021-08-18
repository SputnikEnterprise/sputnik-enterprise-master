

Namespace Notifying
	Public Class NotifyData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property NotifyHeader As String
		Public Property NotifyComments As String
		Public Property NotifyArt As NotifyEnum
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String
		Public Property Checked As Boolean

		Public Enum NotifyEnum
			COMMON
			DOCUMENTSCANNING
			APPLICATION

			EMPLOYEEWOS
			CUSTOMERWOS
			WOSMAIL

			STATIONUPDATE
			FTPUPDATE

			PVLCATEGORIES
			PVLVERSIONCHECK

			GAVFLDATA
			PVLADDRESS

			SYSTEMUPDATE
			SCANFILEINFO
			SCANERROR
		End Enum

		Public ReadOnly Property WhoCreated_FullData As String
			Get
				Return String.Format("{0}, {1}", CreatedOn, CreatedFrom)
			End Get
		End Property

		Public ReadOnly Property WhoChecked_FullData As String
			Get
				If CheckedOn.HasValue Then
					Return String.Format("{0}, {1}", CheckedOn, CheckedFrom)
				Else
					Return String.Empty
				End If
			End Get
		End Property


	End Class

End Namespace


Public Class FoundedeMailData

	Public Property ID As Integer?
	Public Property customer_id As String
	Public Property recNr As Integer?
	Public Property KDNr As Integer?
	Public Property ZHDNr As Integer?
	Public Property MANr As Integer?

	Public Property email_to As String
	Public Property email_from As String
	Public Property email_subject As String
	Public Property email_body As String
	Public Property messageID As String

	Public Property createdon As DateTime?
	Public Property createdfrom As String


End Class


Public Class FoundedJOBCHVacanciesData

  Public Property ID As Integer?
  Public Property customerguid As String
  Public Property organisationid As Integer?

End Class


Public Class FoundedOSTJOBCHVacanciesData

  Public Property ID As Integer?
  Public Property customerguid As String

End Class


Public Class PaymentServicesData

  Public Property ID As Integer?
  Public Property customerguid As String
  Public Property jobid As String

End Class


Public Class ProviderViewData

	Public Property ID As Integer
	Public Property Customer_ID As String
	Public Property ProviderName As String
	Public Property AccountName As String
	Public Property UserName As String
	Public Property UserData As String

End Class

Public Class CustomerServicesViewData

	Public Property customer_ID As String
	Public Property customer_Name As String
	Public Property DeniedServiceName As String

End Class

Public Class JobplattformCounterViewData

	Public Property ID As Integer
	Public Property Customer_ID As String
	Public Property WOS_ID As String
	Public Property OwnCounter As Integer
	Public Property JobsCHCounter As Integer
	Public Property OstJobCounter As Integer
	Public Property JobChannelPriorityCounter As Integer

End Class

Public Class VacancyStateViewData

	Public Property ID As Integer
	Public Property Customer_ID As String
	Public Property WOS_ID As String
	Public Property VacancyNumber As Integer?
	Public Property PublishedOn As DateTime?
	Public Property PublishedUser As String

End Class

Public Class LocationGoordinateViewData
	Public Property ID As Integer?
	Public Property CountryCode As String
	Public Property Postcode As String
	Public Property PlaceName As String
	Public Property AdminName1 As String
	Public Property AdminCode1 As String
	Public Property AdminName2 As String
	Public Property AdminCode2 As String
	Public Property AdminName3 As String
	Public Property AdminCode3 As String
	Public Property Longitude As Double?
	Public Property Latitude As Double?
	Public Property Accuracy As Integer?

End Class

Public Class CVLBaseTableViewData

	Public Property ID As Integer
	Public Property Code As String
	Public Property Bez_DE As String
	Public Property Translated_Value As String

End Class

Public Class GeoCoordinateDataViewData
	Public Property ID As Integer?
	Public Property CountryCode As String
	Public Property Postcode As String
	Public Property PlaceName As String
	Public Property AdminName1 As String
	Public Property AdminCode1 As String
	Public Property AdminName2 As String
	Public Property AdminCode2 As String
	Public Property AdminName3 As String
	Public Property AdminCode3 As String
	Public Property Longitude As Double?
	Public Property Latitude As Double?
	Public Property Accuracy As Integer?

End Class


Public Class QualificationData
	Public Property Code As Integer?
	Public Property TranslatedValue As String
	Public Property MP As Boolean

End Class


Public Class TaxData

#Region "Constructor"

	Public Sub New()

	End Sub

	''' <summary>
	''' The constructor.
	''' </summary>
	''' <param name="data">The data.</param>
	Public Sub New(ByVal data As List(Of TaxDataItem))
		Me.Data = data

	End Sub

#End Region

#Region "Public Properties"

	Public Property Data As List(Of TaxDataItem)

#End Region

End Class

Public Class TaxDataItem
	Public Property Kanton As String
	Public Property Gruppe As String
	Public Property Kinder As Short
	Public Property Kirchensteuer As String
End Class


Public Class TaxCodeData

	Public Property ID As Integer
	Public Property Rec_Value As String
	Public Property Translated_Value As String

End Class

Public Class CommunityData

	Public Property ID As Integer?
	Public Property HistoricNumber As Integer?
	Public Property Canton As String
	Public Property BezirkNumber As Integer?
	Public Property BezirkName As String
	Public Property BFSNumber As Integer?
	Public Property Translated_Value As String

	Public ReadOnly Property ViewData As String
		Get
			Return String.Format("{0} - {1}", BFSNumber, Translated_Value)
		End Get
	End Property
End Class


Public Class TaxChurchCodeData
	Public Property ID As Integer
	Public Property Rec_Value As String
	Public Property Translated_Value As String

End Class

Public Class EmploymentTypeData
	Public Property ID As Integer
	Public Property Rec_Value As String
	Public Property Translated_Value As String

End Class

Public Class TypeOfStayData
	Public Property ID As Integer
	Public Property Rec_Value As String
	Public Property Translated_Value As String

End Class

Public Class PermissionData
	Public Property ID As Integer
	Public Property RecNr As Integer
	Public Property Rec_Value As String
	Public Property Code As String
	Public Property Translated_Value As String

End Class

Public Class BankstammViewData
	Public Property ClearingNumber As String
	Public Property BankName As String
	Public Property Postcode As String
	Public Property Location As String
	Public Property Swift As String
	Public Property Telephone As String
	Public Property Telefax As String
	Public Property PostAccount As String

	Public ReadOnly Property PostcodeAndLocation As String
		Get
			Return String.Format("{0} {1}", Postcode, Location)
		End Get
	End Property

End Class

Public Class IBANVersionResultViewData
	Public Property MajorVersion As Integer
	Public Property MinorVersion As Integer
	Public Property ValidUntil As String
End Class

''' <summary>
''' IBAN convert result.
''' </summary>
Public Class IBANConvertResultViewData
	Public Property IBAN As String
	Public Property PC As String
	Public Property ResultCode As Integer
	Public Property Success As Boolean
End Class


''' <summary>
''' IBAN Decode result.
''' </summary>
Public Class IBANDecodeResultViewData
	Public Property Landcode As String
	Public Property BankID As String
	Public Property Kontonummer As String
	Public Property ResultCode As IBANDecodeResultCode
End Class

Public Enum IBANDecodeResultCode
	Success
	InvalidIBAN
	UnkownIBANCountryCode
	Failure
End Enum


Public Class VacancyJobCHPeripheryViewData
	Public Property ID As Integer
	Public Property ID_Parent As Integer
	Public Property RecNr As Integer
	Public Property TranslatedLabel As String

End Class

