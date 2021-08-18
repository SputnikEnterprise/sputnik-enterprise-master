Namespace Customer.DataObjects

  ''' <summary>
  ''' Responsible person master data.
  ''' </summary>
  Public Class ResponsiblePersonMasterData

    Public Property ID As Integer
    Public Property CustomerNumber As String
    Public Property Salutation As String
    Public Property Lastname As String
    Public Property Firstname As String
    Public Property Department As String
    Public Property Position As String
    Public Property Telephone As String
    Public Property Telefax As String
    Public Property MobilePhone As String
    Public Property Email As String
    Public Property Facebook As String
		Public Property LinkedIn As String
		Public Property Xing As String

		Public Property Birthdate As DateTime?
    Public Property Interests As String
    Public Property Company1 As String
    Public Property PostOfficeBox As String
    Public Property Street As String
    Public Property Postcode As String
    Public Property CountryCode As String
    Public Property Location As String
    Public Property Advisor As String
    Public Property FirstContactDate As DateTime?
    Public Property LastContactDate As DateTime?
    'Public Property KDZuNr As Integer?
    Public Property State1 As String
    Public Property KDZComments As String
    Public Property KDZHowKontakt As String
    Public Property State2 As String
    Public Property SalutationForm As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property RecordNumber As Integer?
    Public Property Comments As String
    Public Property Telefax_Mailing As Boolean
    Public Property SMS_Mailing As Boolean
    Public Property Email_Mailing As Boolean
    Public Property TransferedGuid As String
    Public Property TermsAndConditions_WOS As String

    ' Additional data
    Public Property TranslatedSalutation As String
    Public Property TranslatedSalutationForm As String

		Public ReadOnly Property ResponsiblePersonFullnameWithComma As String
			Get
				Return String.Format("{1}, {0}", Firstname, Lastname)
			End Get
		End Property

		Public ReadOnly Property ResponsiblePersonFullname As String
			Get
				Return String.Format("{0} {1}", Firstname, Lastname)
			End Get
		End Property

	End Class

End Namespace
