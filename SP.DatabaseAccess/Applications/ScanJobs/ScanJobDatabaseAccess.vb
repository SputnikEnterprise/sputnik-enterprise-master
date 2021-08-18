
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.ScanJob.DataObjects
Imports System.Text
'Imports System.Transactions


Namespace ScanJob


	Public Class ScanJobDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IScanJobDatabaseAccess


#Region "Constructor"

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
			MyBase.New(connectionString, translationLanguage)

		End Sub

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
			MyBase.New(connectionString, translationLanguage)
		End Sub

#End Region


	End Class


End Namespace
