Namespace Common.DataObjects

	''' <summary>
	''' Country data.
	''' </summary>
	Public Class CountryData
		Public Property ID As Integer
		Public Property Name As String
		Public Property Code As String

		Public ReadOnly Property CountryDataViewData As String
			Get
				Return String.Format("{0} - {1}", Code, Name)
			End Get
		End Property
	End Class

End Namespace