Public Class GAVDataPerNumberCantonAndGroup

  Private m_Data(50, 26, 26) As GAVData

  Public Sub New()

		Try

			For i As Integer = 0 To 50
				For j As Integer = 0 To 26
					For k As Integer = 0 To 26
						'm_Data(i, j, k) = Nothing
						m_Data(i, j, k) = New GAVData
					Next
				Next
			Next

		Catch ex As System.OutOfMemoryException
			Throw New Exception(String.Format("GAVDataPerNumberCantonAndGroup: {0}.", ex.ToString))
		Catch ex As Exception
			Throw New Exception(String.Format("GAVDataPerNumberCantonAndGroup: {0}.", ex.ToString))
		End Try

	End Sub

  Public Function GetData(ByVal gavNr As Integer, ByVal cantonNr As Integer, ByVal group As Integer) As GAVData
    Return m_Data(gavNr, cantonNr, group)
  End Function

End Class
