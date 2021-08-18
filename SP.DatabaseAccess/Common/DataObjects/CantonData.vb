Namespace Common.DataObjects

  Public Class CantonData

    Public Property GetField As String
    Public Property Description As String

		Public ReadOnly Property CantonViewData As String
			Get
				Return String.Format("{0} - {1}", GetField, Description)
			End Get
		End Property

	End Class

End Namespace
