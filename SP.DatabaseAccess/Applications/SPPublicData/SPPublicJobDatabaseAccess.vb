
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.SPPublicDataJob
Imports System.Text


Namespace SPPublicDataJob


	Public Class SPPublicDataJobDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ISPPublicDataJobDatabaseAccess


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
