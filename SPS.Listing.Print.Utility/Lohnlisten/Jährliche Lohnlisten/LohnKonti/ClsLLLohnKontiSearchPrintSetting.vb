
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility

Public Class ClsLLLohnKontiSearchPrintSetting

	Public Property LohnKontiData As IEnumerable(Of ListingPayrollLohnkontiData)
	Public Property m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Public Property frmhwnd As String
	Public Property SelectedYear As Integer
	Public Property SelectedMonth As Integer?



	Public Property ShowAsDesign As Boolean
	Public Property EmployeeNumber As Integer?
	Public Property ListSortBez As String
	Public Property ListFilterBez As List(Of String)
	Public Property USSignFileName As String

	Public Property PrintJobNumber As String
	Public Property SQL2Open As String

	Public Property ListBez2Print As String

	Public Property ExportData As Boolean?
	Public Property ExportedFiles As List(Of String)
	Public Property ExportedFileName As String



	'Public Property JahrBis2Print As String
	'Public Property MonatVon2Print As String
	'Public Property MonatBis2Print As String

	'Public Property TotalSuva As New List(Of Decimal)
	'Public Property esAnzM As Integer
	'Public Property esAnzF As Integer

	'Public Property LLShowESAnzJahr As Boolean
	'Public Property LLESAnzJahr As Integer
	'Public Property MDsuva_hl As Decimal
	'Public Property ausgleichsNr As String

	'Public Property SecSuvaCode As New Dictionary(Of String, String)
	'Public Property PrintUVGWithSuvaRekap As Boolean

End Class
