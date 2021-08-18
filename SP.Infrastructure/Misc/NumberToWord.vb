Namespace Misc

  ''' <summary>
  ''' Converts a number to a text.
  ''' </summary>
  '''<remarks>Algorithm is taken from http://www-i1.informatik.rwth-aachen.de/~algorithmus/Algorithmen/algo4/algo04.pdf </remarks>
  Public Class NumberToWord

    ''' <summary> 
    ''' Converts a number to german text.
    ''' </summary>
    ''' <param name="value">The number.</param>
    ''' <returns>The german text.</returns>
    ''' <remarks>Only number smaller than 1000000000000000 are supported. </remarks>
    Public Function ConvertNumberToGermanText(ByVal value As Decimal) As String

      value = Math.Abs(value)
      value = Math.Floor(value)

      If value >= 1000000000000000 Then
        ' Zahlen >= 1 Billiarde werden nicht unterstützt
        Throw New Exception("Number >= 1000000000000000 are not supported.")
      End If

      If value = 0 Then
        Return "null"
      End If

      Dim i As Integer = -1 ' i ist der Index der zuletzt gefundenen Gruppe

      'Maximal fünf Gruppen: 1 000 000 000 000 000
      Dim blocksOfThree(5) As Integer

      While value > 0
        i = i + 1 ' Index der neuen Gruppe
        Dim rest = value Mod 1000 ' Rest zur ...
        value = value \ 1000 ' ... Division durch 1000
        blocksOfThree(i) = rest
      End While

      Dim text As String = ""

      Dim singular() As String = New String() {"s", "tausend", "emillion", "emilliarde", "ebillion"}
      Dim plural() As String = New String() {String.Empty, "tausend", "millionen", "milliarden", "billionen"}
      Dim nulleins() As String = New String() {"s", "stausend", "smillionen", "smilliarden", "sbillionen"}

      ' Alle Gruppen in Textform umwandeln
      While i >= 0

        If blocksOfThree(i) > 0 Then
          text = text & ConvertGroupOfThreeDigitsToText(blocksOfThree(i))

          Dim textGroupOfThree = blocksOfThree(i).ToString()
          Dim doesGroupEndsWithNullEins = textGroupOfThree.Length = 3 AndAlso textGroupOfThree(1) = "0" AndAlso textGroupOfThree(2) = "1"

          If blocksOfThree(i) = 1 Then
            text = text & singular(i)
          ElseIf doesGroupEndsWithNullEins Then
            text = text & nulleins(i)
          Else
            text = text & plural(i)
          End If

        End If
        i = i - 1

      End While

      Return text

    End Function

    ''' <summary>
    ''' Converts a group of three digits to text.
    ''' </summary>
    ''' <param name="value">The three digit value.</param>
    ''' <returns>The text.</returns>
    Private Function ConvertGroupOfThreeDigitsToText(ByVal value As Decimal)


      Dim lessThan20 As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)

      lessThan20.Add(1, "ein")
      lessThan20.Add(2, "zwei")
      lessThan20.Add(3, "drei")
      lessThan20.Add(4, "vier")
      lessThan20.Add(5, "fünf")
      lessThan20.Add(6, "sechs")
      lessThan20.Add(7, "sieben")
      lessThan20.Add(8, "acht")
      lessThan20.Add(9, "neun")
      lessThan20.Add(10, "zehn")
      lessThan20.Add(11, "elf")
      lessThan20.Add(12, "zwölf")
      lessThan20.Add(13, "dreizehn")
      lessThan20.Add(14, "vierzehn")
      lessThan20.Add(15, "fünfzehn")
      lessThan20.Add(16, "sechzehn")
      lessThan20.Add(17, "siebzehn")
      lessThan20.Add(18, "achtzehn")
      lessThan20.Add(19, "neunzehn")

      Dim tenner As New Dictionary(Of Integer, String)

      tenner.Add(2, "zwanzig")
      tenner.Add(3, "dreissig")
      tenner.Add(4, "vierzig")
      tenner.Add(5, "fünfzig")
      tenner.Add(6, "sechzig")
      tenner.Add(7, "siebzig")
      tenner.Add(8, "achtzig")
      tenner.Add(9, "neunzig")

      Dim h = value \ 100 ' die Hunderter-Ziffer
      Dim r = value Mod 100 ' der Rest kleiner als 100
      Dim z = r \ 10 ' die Zehner-Ziffer
      Dim e = value Mod 10 ' die Einzer-Ziffer
      Dim erg = String.Empty ' Text anfangs leer (bleibt leer, fals zahl = 0)

      ' Zuerst vohandene Hunderter anfügen:
      If h > 0 Then
        erg = erg & lessThan20(h) & "hundert"
      End If

      ' Dann den Rest kleiner als 100 umwandeln:
      If r > 0 Then
        If r < 20 Then
          ' Kleine Zahlen einfach aus der Tabelle nehmen
          erg = erg & lessThan20(r)
        Else
          ' Wenn 10er oder 1er fehlen, dann kein "und"
          If e > 0 Then erg = erg & lessThan20(e)
          If e > 0 And z > 0 Then erg = erg & "und"
          If z > 0 Then erg = erg & tenner(z)
        End If
      End If

      Return erg
    End Function
  End Class

End Namespace
