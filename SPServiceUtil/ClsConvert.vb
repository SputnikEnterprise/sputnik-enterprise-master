

Public Class ClsConvert

  ''' <summary>
  ''' Konvertiert ein Array in einen string
  ''' </summary>
  ''' <param name="Ar">Erwartet ein Stringarray</param>
  ''' <param name="Seperator">Ein Trennzeichen für die Arrays, Standard ist ", "</param>
  Public Function ArrayToString(ByVal ar() As String, Optional ByVal Seperator As String = ", ") As String
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
  Public Overloads Function ConvListObject2String(ByVal lst As List(Of String), Optional ByVal Seperator As String = ", ") As String
    Dim str As New System.Text.StringBuilder
    For i As Integer = 0 To lst.Count - 1
      str.AppendFormat("{0}{1}", lst.Item(i), If(i = lst.Count - 1, "", Seperator))
    Next
    Return str.ToString
  End Function

  ''' <summary>
  ''' Konvertiert eine ListOfString in ein Array
  ''' </summary>
  ''' <param name="lst">Die zu konvertierende Liste.</param>
  Public Overloads Function ConvListObject2Array(ByVal lst As List(Of String)) As String()
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
  Public Overloads Function ConvArray2ListObject(ByVal ar() As String) As List(Of String)
    Dim StringList As New List(Of String)
    StringList.AddRange(ar)
    'For i As Integer = 0 To ar.Length - 1
    '    StringList.Add(ar(i))
    'Next
    Return StringList
  End Function

  ''' <summary>
  ''' Konvertiert Listbox-Einträge in eine ListOfString
  ''' und gibt sie zurück
  ''' </summary>
  ''' <param name="lst">Die Listbox, aus der die einträge entnommen werden sollen.</param>
  Public Overloads Function ConvListObject2ListOfString(ByRef lst As ListBox) As List(Of String)
    Dim StringList As New List(Of String)

    For i As Integer = 0 To lst.Items.Count - 1
      StringList.Add(CStr(lst.Items.Item(i)))
    Next

    Return StringList
  End Function

  ''' <summary>
  ''' Konvertiert ComboBox-Einträge in eine ListOfString
  ''' und gibt sie zurück
  ''' </summary>
  ''' <param name="lst">Die ComboBox, aus der die einträge entnommen werden sollen.</param>
  Public Overloads Function ConvListObject2ListOfString(ByRef lst As ComboBox) As List(Of String)
    Dim StringList As New List(Of String)

    For i As Integer = 0 To lst.Items.Count - 1
      StringList.Add(CStr(lst.Items.Item(i)))
    Next

    Return StringList
  End Function

#Region "ADD X to Listbox"
  ''' <summary>
  ''' Fügt einer Listbox eine ListOfString hinzu
  ''' </summary>
  ''' <param name="lst">Die Listbox, in die geschreiben werden soll.</param>
  ''' <param name="strngList">Die ListOfString, die in die Listbox geschrieben werden soll.</param>
  Public Overloads Function ConvListOfString2ListObject(ByRef lst As ListBox, _
                                                        ByVal strngList As List(Of String)) As ListBox
    For i As Integer = 0 To strngList.Count - 1
      lst.Items.Add(strngList.Item(i))
    Next

    Return lst
  End Function

  ''' <summary>
  ''' Fügt einer Listbox ein Stringarray hinzu
  ''' </summary>
  ''' <param name="lst">Die Listbox, in die geschreiben werden soll.</param>
  ''' <param name="strngList">Das Stringarray, das in die Listbox geschrieben werden soll.</param>
  Public Overloads Function ConvListOfString2ListObject(ByRef lst As ListBox, ByVal strngList() As String) As ListBox
    For i As Integer = 0 To strngList.Length - 1
      lst.Items.Add(strngList(i))
    Next

    Return lst
  End Function

#End Region

#Region "ADD X to ToolStripComboBox"
  ''' <summary>
  ''' Fügt einer ToolStripComboBox eine ListOfString hinzu
  ''' </summary>
  ''' <param name="lst">Die ToolStripComboBox, in die geschreiben werden soll.</param>
  ''' <param name="strngList">Die ListOfString, die in die ToolStripComboBox geschrieben werden soll.</param>
  Public Overloads Function ConvListOfString2ListObject(ByRef lst As ToolStripComboBox, _
                                                        ByVal strngList As List(Of String)) As ToolStripComboBox
    For i As Integer = 0 To strngList.Count - 1
      lst.Items.Add(strngList.Item(i))
    Next

    Return lst
  End Function

  ''' <summary>
  ''' Fügt einer ToolStripComboBox ein Stringarray hinzu
  ''' </summary>
  ''' <param name="lst">Die ToolStripComboBox, in die geschreiben werden soll.</param>
  ''' <param name="strngList">Das Stringarray, das in die ToolStripComboBox geschrieben werden soll.</param>
  Public Overloads Function ConvListOfString2ListObject(ByRef lst As ToolStripComboBox, _
                                                        ByVal strngList() As String) As ToolStripComboBox
    For i As Integer = 0 To strngList.Length - 1
      lst.Items.Add(strngList(i))
    Next

    Return lst
  End Function
#End Region

#Region "ADD X to ComboBox"
  ''' <summary>
  ''' Fügt einer ComboBox eine ListOfString hinzu
  ''' </summary>
  ''' <param name="lst">Die ComboBox, in die geschreiben werden soll.</param>
  ''' <param name="strngList">Die ListOfString, die in die Listbox geschrieben werden soll.</param>
  Public Overloads Function ConvListOfString2ListObject(ByRef lst As ComboBox, _
                                                        ByVal strngList As List(Of String)) As ComboBox
    For i As Integer = 0 To strngList.Count - 1
      lst.Items.Add(strngList.Item(i))
    Next

    Return lst
  End Function

  ''' <summary>
  ''' Fügt einer ComboBox ein Stringarray hinzu
  ''' </summary>
  ''' <param name="lst">Die ComboBox, in die geschreiben werden soll.</param>
  ''' <param name="strngList">Das Stringarray, das in die Listbox geschrieben werden soll.</param>
  Public Overloads Function ConvListOfString2ListObject(ByRef lst As ComboBox, _
                                                        ByVal strngList() As String) As ComboBox
    For i As Integer = 0 To strngList.Length - 1
      lst.Items.Add(strngList(i))
    Next

    Return lst
  End Function
#End Region


End Class
