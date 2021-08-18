Public Class GAVDataPerNumber

  Private m_Data(50) As GAVData

  Public Sub New()

    For i As Integer = 0 To 50
      m_Data(i) = New GAVData
    Next

  End Sub

  Public Function GetData(ByVal gavNr As Integer) As GAVData
    Return m_Data(gavNr)
  End Function

End Class
