Namespace Customer.DataObjects

  ''' <summary>
  ''' Propose data (Propose).
  ''' </summary>
  Public Class ProposeData
    Public Property ID As Integer
    Public Property CustomerNumber As Integer?
    Public Property ProposeNumber As Integer?
    Public Property ResponsiblePersonRecordNumber As Integer?
    Public Property Description As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property P_State As String

		Public Property EmployeeNumber As Integer?
		Public Property EmployeeFirstname As String
		Public Property EmployeeLastname As String


		Public ReadOnly Property EmployeeFullname As String
			Get
				Return String.Format("{0}, {1}", EmployeeFirstname, EmployeeLastname)
			End Get
		End Property


	End Class

End Namespace