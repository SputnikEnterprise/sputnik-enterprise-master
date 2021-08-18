Namespace Common.DataObjects

  ''' <summary>
  ''' Advisor data.
  ''' </summary>
  Public Class AdvisorData
		Public Property UserMDNr As Integer?
		Public Property UserNumber As Integer
    Public Property Firstname As String
    Public Property Lastname As String
    Public Property Salutation As String
    Public Property KST As String
		Public Property KST1 As String
		Public Property KST2 As String
		Public Property UserGuid As String
		Public Property Deactivated As Boolean?


		Public Property UserLoginname As String
		Public Property UserLoginPassword As String
		Public Property UserBusinessBranch As String
		Public Property UserFiliale As String
		Public Property UserFTitel As String
		Public Property UserSTitel As String
		Public Property UserTelefon As String
		Public Property UserTelefax As String
		Public Property UserMobile As String
		Public Property UsereMail As String
		Public Property UserLanguage As String
		Public Property UserMDTelefon As String
		Public Property UserMDDTelefon As String
		Public Property UserMDTelefax As String
		Public Property UserMDeMail As String
		Public Property UserMDGuid As String
		Public Property UserMDName As String
		Public Property UserMDName2 As String
		Public Property UserMDName3 As String
		Public Property UserMDPostfach As String
		Public Property UserMDStrasse As String
		Public Property UserMDPLZ As String
		Public Property UserMDOrt As String
		Public Property MDCanton As String
		Public Property UserMDLand As String
		Public Property UserMDHomepage As String

		Public ReadOnly Property UserFullname As String
			Get
				Return Strings.Trim(Lastname) + ", " + Strings.Trim(Firstname)
			End Get
		End Property
    Public ReadOnly Property UserFullnameReversedWithoutComma As String
      Get
        Return Strings.Trim(Firstname) + " " + Strings.Trim(Lastname)
      End Get
    End Property
  End Class












End Namespace