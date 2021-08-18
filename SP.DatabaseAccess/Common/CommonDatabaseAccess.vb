
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language
Namespace Common

  ''' <summary>
  ''' Common database access class.
  ''' </summary>
  Public Class CommonDatabaseAccess
    Inherits DatabaseAccessBase
    Implements ICommonDatabaseAccess

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