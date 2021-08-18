
Public Interface IClsWOS

  Function DeleteDocFromWOS() As String


End Interface

Public Class ClsWOSInfomation

  Public Property SelectedMANr As Integer
  Public Property SelectedKDNr As Integer
  Public Property SelectedZHDNr As Integer

  Public Property SelectedESNr As Integer
  Public Property SelectedESLohnNr As Integer

  Public Property SelectedRPNr As Integer
  Public Property SelectedRPLNr As Integer

  Public Property SelectedFilename As String

  Public Property SelectedKDGuid As String
  Public Property SelectedZHDGuid As String
  Public Property SelectedMAGuid As String

  Public Property SelectedDocGuid As String

  Public Property SendRPToKDWOS As Boolean
  Public Property SendRPToMAWOS As Boolean

End Class