Namespace Common.DataObjects

  Public Class CivilStateData

		Public Property recid As Integer
		Public Property GetField As String
    Public Property Description As String

		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

    ''' <summary>
    ''' Gets  or sets the translated civil state.
    ''' </summary>
    ''' <returns>Returns the translated civil state text.</returns>
    Public Property TranslatedCivilState As String

		Public ReadOnly Property CivilStateViewData
			Get
				Return String.Format("{0} -{1}", GetField, TranslatedCivilState)
			End Get
		End Property


	End Class

End Namespace
