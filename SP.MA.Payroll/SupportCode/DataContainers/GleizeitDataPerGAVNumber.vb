Public Class GleizeitDataPerGAVNumber

  Private m_Data(50) As GleitzeitData

  Public Sub New()

    For i As Integer = 0 To 49
      m_Data(i) = New GleitzeitData
    Next

  End Sub

  Public Function GetData(ByVal gavNumber As Integer) As GleitzeitData
    Return m_Data(gavNumber)
  End Function

End Class
