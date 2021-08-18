Public Class GAVDataPerNumberAndCanton

  Private m_Data(50, 26) As GAVData

  Public Sub New()

    For i As Integer = 0 To 50
      For j As Integer = 0 To 26
        m_Data(i, j) = New GAVData
      Next
    Next

  End Sub

  Public Function GetData(ByVal gavNr As Integer, ByVal cantonNr As Integer) As GAVData
    Return m_Data(gavNr, cantonNr)
  End Function

End Class
