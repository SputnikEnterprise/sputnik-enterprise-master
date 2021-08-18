
Namespace SPConverter

  Public Class ClsConvert

    ''' <summary>
    ''' Konvertiert ein Array in einen string
    ''' </summary>
    ''' <param name="Ar">Erwartet ein Stringarray</param>
    ''' <param name="Seperator">Ein Trennzeichen für die Arrays, Standard ist ", "</param>
    Public Shared Function ArrayToString(ByVal ar() As String, Optional ByVal Seperator As String = ", ") As String
      Dim AtS As New System.Text.StringBuilder
      For i As Integer = 0 To ar.Length - 1
        AtS.Append(ar(i))
        If i <> ar.Length - 1 Then
          AtS.Append(Seperator)
        End If
      Next

      Return AtS.ToString
    End Function

    Public Shared Function ArrayToString(ByVal ar() As Integer, Optional ByVal Seperator As String = ", ") As String
      Dim AtS As New System.Text.StringBuilder
      For i As Integer = 0 To ar.Length - 1
        AtS.Append(ar(i))
        If i <> ar.Length - 1 Then
          AtS.Append(Seperator)
        End If
      Next

      Return AtS.ToString
    End Function

    Public Shared Function ArrayToString(ByVal ar() As Short, Optional ByVal Seperator As String = ", ") As String
      Dim AtS As New System.Text.StringBuilder
      For i As Integer = 0 To ar.Length - 1
        AtS.Append(ar(i))
        If i <> ar.Length - 1 Then
          AtS.Append(Seperator)
        End If
      Next

      Return AtS.ToString
    End Function

    ''' <summary>
    ''' Konvertiert eine ListOfString in einen String
    ''' </summary>
    ''' <param name="lst">Die zu konvertierende Liste.</param>
    ''' <param name="Seperator">Ein Trennzeichen für die Arrays, Standard ist ", "</param>
    Public Overloads Shared Function ConvListObject2String(ByVal lst As List(Of String), Optional ByVal Seperator As String = ", ") As String
      Dim str As New System.Text.StringBuilder
      For i As Integer = 0 To lst.Count - 1
        str.AppendFormat("{0}{1}", lst.Item(i), If(i = lst.Count - 1, "", Seperator))
      Next
      Return str.ToString
    End Function

    Public Overloads Shared Function ConvListObject2String(ByVal lst As List(Of Integer), Optional ByVal Seperator As String = ", ") As String
      Dim str As New System.Text.StringBuilder
      For i As Integer = 0 To lst.Count - 1
        str.AppendFormat("{0}{1}", CInt(lst.Item(i)), If(i = lst.Count - 1, "", Seperator))
      Next
      Return str.ToString
    End Function

    Public Overloads Shared Function ConvListObject2String(ByVal lst As List(Of Short), Optional ByVal Seperator As String = ", ") As String
      Dim str As New System.Text.StringBuilder
      For i As Integer = 0 To lst.Count - 1
        str.AppendFormat("{0}{1}", CInt(lst.Item(i)), If(i = lst.Count - 1, "", Seperator))
      Next
      Return str.ToString
    End Function

    ''' <summary>
    ''' Konvertiert eine ListOfString in ein Array
    ''' </summary>
    ''' <param name="lst">Die zu konvertierende Liste.</param>
    Public Overloads Shared Function ConvListObject2Array(ByVal lst As List(Of String)) As String()
      'Dim Ar(lst.Count - 1) As String

      'For i As Integer = 0 To lst.Count - 1
      '    Ar(i) = lst.Item(i)
      'Next
      Return lst.ToArray
    End Function

    ''' <summary>
    ''' Konvertiert ein Array in eine ListOfString
    ''' </summary>
    ''' <param name="ar">Das zu konvertierende Array.</param>
    Public Overloads Shared Function ConvArray2ListObject(ByVal ar() As String) As List(Of String)
      Dim StringList As New List(Of String)
      StringList.AddRange(ar)
      'For i As Integer = 0 To ar.Length - 1
      '    StringList.Add(ar(i))
      'Next
      Return StringList
    End Function

  End Class

End Namespace
