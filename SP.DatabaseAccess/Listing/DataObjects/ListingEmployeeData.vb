
Namespace Listing.DataObjects

	Public Class EmployeeTranferData

		Public Property SourceEmployeeNumber As Integer?
		Public Property SourceDataBaseName As String

		Public Property DestNewEmployeeNumber As Integer?
		Public Property DestEmployeeOffsetNumber As Integer?
		Public Property DestMDNumber As Integer
		Public Property DestCustomerID As String

	End Class

	Public Class CustomerTranferData

		Public Property SourceCustomerNumber As Integer?
		Public Property DestNewCustomerNumber As Integer?
		Public Property DestCustomerOffsetNumber As Integer?
		Public Property DestMDNumber As Integer
		Public Property DestCustomerID As String
		Public Property SourceDataBaseName As String

	End Class

	Public Class CResponsiblePersonTranferData

		Public Property SourceCustomerNumber As Integer?
		Public Property SourceCResponsibleNumber As Integer?
		Public Property DestCustomerNumber As Integer?
		Public Property DestNewCResponsibleNumber As Integer?
		Public Property DestMDNumber As Integer
		Public Property DestCustomerID As String
		Public Property SourceDataBaseName As String

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

	Public Class AnyStringValueData
		Public Property FieldValue As String

	End Class

	Public Class TrueFalseValueData
		Public Property FieldValue As Integer
		Public Property ValueLabel As String

	End Class


	Public Class DowntimeData

		Public Property MDNr As Integer
		Public Property EmployeeNumber As Integer
		Public Property CustomerNumber As Integer?
		Public Property PayrollMonth As Integer?
		Public Property PayrollYear As Integer?
		Public Property PayrollNumber As Integer?
		Public Property NeedAttention As Boolean?
		Public Property LALabel As String
		Public Property Customername As String
		Public Property EmployeeFirstname As String
		Public Property EmployeeLastname As String
		Public Property EmployeeSocialSecurityNumber As String
		Public Property EmployeeBirthdate As Date?
		Public Property TargetHours As Decimal?
		Public Property DowntimeHours As Decimal?
		Public Property DowntimeProcentage As Decimal?
		Public Property AHVBasis As Decimal?
		Public Property DowntimeAHVBasis As Decimal?
		Public Property DowntimeCompensation As Decimal?
		Public Property DowntimeCompensationAG As Decimal?
		Public Property TotalCompensation As Decimal?


		Public ReadOnly Property EmployeeFullname As String
			Get
				Return String.Format("{1}, {0}", EmployeeFirstname, EmployeeLastname)
			End Get
		End Property


	End Class


End Namespace
