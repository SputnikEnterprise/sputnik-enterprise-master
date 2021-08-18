
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.EMailJob.DataObjects
Imports System.Text
'Imports System.Transactions


Namespace EMailJob


	Public Class EMailJobDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IEMailJobDatabaseAccess


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

