
Public Class ShortMessage

  Public Property ReceiverId As Integer

  Public Property Address As String
  Public Property Message As String

  Public Property JobId As String

  Public Property ResponseCode As Long
  Public Property ResponseText As String

  Public Property SendState As Long
  Public Property ErrorState As Long

  Public Property FinishDate As Date

  Public Property PointsUsed As Double
  Public Property AnswerAddress As String


  Sub New()
    ResponseCode = 0
    SendState = 0
    ErrorState = 0
    ResponseText = String.Empty
    PointsUsed = 0.0
  End Sub

  Public Sub UpdateStatus(ByVal status As ShortMessage)
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

  Public Function Clone() As ShortMessage
    Return DirectCast(Me.MemberwiseClone(), ShortMessage)
  End Function

End Class
