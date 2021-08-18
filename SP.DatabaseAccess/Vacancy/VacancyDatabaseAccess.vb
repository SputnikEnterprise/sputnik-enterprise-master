
Imports System.Data.SqlClient

Namespace Vacancy

  Public Class VacancyDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IVacancyDatabaseAccess

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="translationLanguage">The translation language.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
      MyBase.New(connectionString, translationLanguage)

    End Sub

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="translationLanguage">The translation language.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
      MyBase.new(connectionString, translationLanguage)
    End Sub

#End Region


	End Class

End Namespace
