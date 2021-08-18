Imports TinyMessenger

Namespace Messaging

  Public Class MessageService

#Region "Pirvate Fields"

    Private m_TinyMessengerHub As TinyMessengerHub

#End Region

#Region "Shared Fields"

    Private Shared m_Instance As MessageService

#End Region

#Region "Construcor"

    Private Sub New()
      m_TinyMessengerHub = New TinyMessengerHub
    End Sub

#End Region

#Region "Properties"

    Public Shared ReadOnly Property Instance As MessageService
      Get

        If m_Instance Is Nothing Then
          m_Instance = New MessageService
        End If


        Return m_Instance

      End Get

    End Property


    Public ReadOnly Property Hub As ITinyMessengerHub
      Get
        Return m_TinyMessengerHub
      End Get
    End Property

#End Region

  End Class

End Namespace
