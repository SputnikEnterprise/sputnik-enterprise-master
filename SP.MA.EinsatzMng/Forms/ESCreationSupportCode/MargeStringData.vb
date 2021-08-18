
' Marge string data.
Public Class MargeStringData

  Public Property MargeOhneBVG As Decimal ' Element 0
  Public Property MargeMitBVG As Decimal ' Element 1
  Public Property AvgMarge As Decimal ' Element 2
  Public Property AvgMargeInProz As Decimal ' Element 3
  Public Property AGBeitragOhneBVG As Decimal ' Element 4
  Public Property AGBeitragWithBVG As Decimal ' Element 5
  Public Property MargeOhneBVGInProz As Decimal ' Element 6
  Public Property MargeWithBVGInProz As Decimal ' Element 7

  ' Add additional properties here if required


  Public Property CompleteMargeString As String

  ''' <summary>
  ''' Helper varialbe to used while parsing GAV values from a string.
  ''' </summary>
  Private m_Index As Integer = 0

  ''' <summary>
  ''' Fills the object form a string value.
  ''' </summary>
  ''' <param name="margeDataString">The marge data string</param>
  Public Sub FillFromString(ByVal margeDataString As String)

    CompleteMargeString = margeDataString

    Dim tokens = margeDataString.Split("¦")
		If tokens.Count <= 1 Then Return

		Try
			m_Index = 0

			MargeOhneBVG = Convert.ToDecimal(tokens(PostIncIndex()))
			MargeMitBVG = Convert.ToDecimal(tokens(PostIncIndex()))
			AvgMarge = Convert.ToDecimal(tokens(PostIncIndex()))
			AvgMargeInProz = Convert.ToDecimal(tokens(PostIncIndex()))
			AGBeitragOhneBVG = Convert.ToDecimal(tokens(PostIncIndex()))
			AGBeitragWithBVG = Convert.ToDecimal(tokens(PostIncIndex()))
			MargeOhneBVGInProz = Convert.ToDecimal(tokens(PostIncIndex()))
			MargeWithBVGInProz = Convert.ToDecimal(tokens(PostIncIndex()))

			m_Index = 0

		Catch ex As Exception

		End Try

  End Sub

  ''' <summary>
  ''' Post increment index (like i++ in C#)
  ''' </summary>
  ''' <returns>Old index value.</returns>
  ''' <remarks>Incements index by one but returns old value.</remarks>
  Private Function PostIncIndex() As Integer
    m_Index = m_Index + 1

    Return m_Index - 1
  End Function

End Class
