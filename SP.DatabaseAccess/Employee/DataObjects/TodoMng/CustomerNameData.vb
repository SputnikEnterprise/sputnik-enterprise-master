Namespace Employee.DataObjects.TodoMng

  ''' <summary>
  ''' Customer data.
  ''' </summary>
  Public Class CustomerNameData
    Public Property KDNr As Integer
    Public Property Firma1 As String
    Public Property Firma2 As String
    Public Property Firma3 As String
    Public Property Postfach As String
    Public Property Strasse As String
    Public Property PLZ As String
    Public Property Ort As String
    Public Property Land As String
    Public ReadOnly Property Firma As String
      Get
        Return Firma1 & IIf(Firma2 = String.Empty, "", " " & Firma2) & ", " & Ort
      End Get
    End Property
  End Class

End Namespace