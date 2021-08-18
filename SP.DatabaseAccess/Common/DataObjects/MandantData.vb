Imports System.IO

Namespace Common.DataObjects

	''' <summary>
	''' Mandant data.
	''' </summary>
	Public Class MandantData
		Public Property ID As Integer
		Public Property MandantNumber As Integer?
		Public Property MandantName1 As String
		Public Property MandantName2 As String
		Public Property MandantCanton As String
		Public Property Street As String
		Public Property Postcode As String
		Public Property Location As String
		Public Property Telephon As String
		Public Property Telefax As String
		Public Property EMail As String
		Public Property Homepage As String

		Public Property MandantDbConnection As String
		Public Property MandantGuid As String

	End Class

	Public Class RootMandantData

		Public Property ID As Integer?
		Public Property MandantGuid As String
		Public Property MandantNumber As Integer?
		Public Property MandantName1 As String
		Public Property MDPath As String
		Public Property Deaktiviert As Boolean?
		Public Property DbName As String
		Public Property DbConnectionstr As String
		Public Property DbServerName As String
		Public Property MDGroupNr As Integer?
		Public Property FileServerPath As String

		Public ReadOnly Property FileServerRootPath As String
			Get
				Return Directory.GetDirectoryRoot(MDPath)
			End Get
		End Property


	End Class


	''' <summary>
	''' CloseMonth data.
	''' </summary>
	Public Class MonthCloseData
		Public Property ID As Integer
		Public Property jahr As Integer?
		Public Property monat As Integer?

		Public Property UserName As String
		Public Property CreatedOn As DateTime?

		Public Property MandantNumber As Integer?
		Public Property MDName As String

	End Class

	''' <summary>
	''' not validated data for closing month.
	''' </summary>
	Public Class NotValidatedData

		Public Property modulname As String
		Public Property modulnr As Integer
		Public Property manr As Integer?

		Public Property startdate As DateTime?
		Public Property maname As String


	End Class

	Public Class SearchQueryTemplateData
		Public Property ID As Integer
		Public Property MenuLabel As String
		Public Property TranslatedLabel As String
		Public Property QueryString As String
		Public Property QueryType As Integer
		Public Property ShowMenuIn As String

	End Class

	Public Enum DeleteResult
		Deleted = 2
		ErrorWhileDelete = 20
	End Enum


End Namespace