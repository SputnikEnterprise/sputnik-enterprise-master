Public Class FaxReceiver

  Public Property Address As String
  Public Property KDNr As String
  Public Property ZHDNr As String

  Public Property JobId As String

  Public Property ResponseCode As Long
  Public Property ResponseText As String

  Public Property SendState As Long
  Public Property ErrorState As Long

  Public Property FinishDate As Date

  Public Property PointsUsed As Double

  Sub New()
    ResponseCode = 0
    SendState = 0
    ErrorState = 0
    ResponseText = String.Empty
    PointsUsed = 0.0
  End Sub

  Public Sub UpdateStatus(ByVal status As FaxReceiver)
    If status.ResponseCode <> -1 Then
      ResponseCode = status.ResponseCode
    End If

    If status.SendState <> -1 Then
      SendState = status.SendState
    End If

    If status.ErrorState <> -1 Then
      ErrorState = status.ErrorState
    End If

    If Not String.IsNullOrEmpty(status.ResponseText) Then
      ResponseText = status.ResponseText
    End If

  End Sub

  Public Function Clone() As FaxReceiver
    Return DirectCast(Me.MemberwiseClone(), FaxReceiver)
  End Function

End Class