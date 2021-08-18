Namespace ES.DataObjects.ESMng

  ''' <summary>
  ''' ES categorization data. (Tab_EsEinstufung)
  ''' </summary>
  Public Class ESCategorizationData

    Public Property ID As Integer
    Public Property Description As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String
		Public Property Result As String

    ''' <summary>
    ''' Gets or sets the translated description.
    ''' </summary>
    ''' <returns>Returns the translated description text.</returns>
    Public Property TranslatedESCategorizationDescription As String

  End Class


End Namespace
