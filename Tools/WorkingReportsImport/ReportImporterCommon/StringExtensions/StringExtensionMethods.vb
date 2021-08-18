'------------------------------------
' File: StringExtensionMethods.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Text.RegularExpressions

Namespace StringExtensions

    ''' <summary>
    ''' Extensions methods for the System.String class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Module StringExtensionMethods

        ''' <summary>
        '''  Checks if a regular expression does match with a string.
        ''' </summary>
        ''' <param name="str">The string object.</param>
        ''' <param name="regularExpression">The regular expression.</param>
        ''' <returns>Boolean truth value if the string machtes with the regular expression.</returns>
        <Extension()>
        Public Function IsRegularExpressionMatching(ByVal str As String, ByVal regularExpression As String) As Boolean

            If Not regularExpression Is Nothing Then

                Dim regularExpressionObject As New Regex(regularExpression)

                Return regularExpressionObject.IsMatch(str)

            End If

            Return False

        End Function

        ''' <summary>
        '''  Truncates a string if its length exceeds a maximal length.
        ''' </summary>
        ''' <param name="str">The string object.</param>
        ''' <param name="maxLength">The maximal length.</param>
        ''' <returns>Orignal string value or truncated string.</returns>
        <Extension()>
        Public Function TruncateLength(ByVal str As String, ByVal maxLength As Integer) As String

            If str.Length <= maxLength Then
                Return str
            Else
                Return str.Substring(0, maxLength)
            End If

        End Function

    End Module

End Namespace


