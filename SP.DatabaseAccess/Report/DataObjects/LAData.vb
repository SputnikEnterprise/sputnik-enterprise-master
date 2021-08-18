Namespace Report.DataObjects

	''' <summary>
	''' LA Data.
	''' </summary>
	Public Class LAData
		Public Property LANr As Decimal?
		Public Property LALoText As String
		Public Property LAOPText As String
		Public Property Sign As String
		Public Property AllowMoreAnzahl As Boolean
		Public Property AllowMoreBasis As Boolean
		Public Property AllowMoreAnsatz As Boolean
		Public Property AllowMoreBetrag As Boolean
		Public Property Rounding As Short?
		Public Property TypeAnzahl As Short?
		Public Property TypeBasis As Short?
		Public Property TypeAnsatz As Short?

		Public Property MABasVar As String
		Public Property FixBasis As Decimal?
		Public Property MAAnsVar As String
		Public Property MAAnzVar As String
		Public Property KDAnzahl As String
		Public Property KDBasis As String
		Public Property KDAnsatz As String
		Public Property FeierInklusiv As Boolean?
		Public Property FerienInklusiv As Boolean?
		Public Property Inklusiv13 As Boolean?
		Public Property FixAnsatz As Decimal?
		Public Property DuppinKD As Boolean?
		Public Property TagesSpesen As Boolean?
		Public Property StdSpesen As Boolean?
		Public Property ProTag As Boolean?
		Public Property GleitTime As Boolean?
		Public Property MoreProz4Fer As Boolean?
		Public Property MoreProz4Feier As Boolean?
		Public Property MoreProz413 As Boolean?
		Public Property MoreProz4FerAmount As Decimal?
		Public Property MoreProz4FeierAmount As Decimal?
		Public Property MoreProz413Amount As Decimal?

		Public Property MWSTPflichtig As Boolean?

		' Uses for translation of LA text
		Public Property RPLTypeForTranslation As RPLType

		Public ReadOnly Property DisplayText As String
			Get

				Select Case RPLTypeForTranslation
					Case RPLType.Employee
						Return String.Format("{0:0.###} - {1}", LANr, LALoText)
					Case RPLType.Customer
						Return String.Format("{0:0.###} - {1}", LANr, LAOPText)
					Case Else
						Return String.Empty

				End Select

			End Get
		End Property

		Public ReadOnly Property TranslatedLaText As String
			Get

				Select Case RPLTypeForTranslation
					Case RPLType.Employee
						Return LALoText
					Case RPLType.Customer
						Return LAOPText
					Case Else
						Return String.Empty

				End Select

			End Get
		End Property

		Public Property Highlight As Boolean

	End Class

End Namespace
