Imports TinyMessenger

Namespace Messaging.Messages

  ''' <summary>
  ''' Request to open a responsible person management form.
  ''' </summary>
  Public Class OpenResponsiblePersonMngRequest
    Inherits TinyMessageBase

#Region "Private Fields"

    ''' <summary>
    ''' The user number.
    ''' </summary>
    Private m_USNr As Integer

    ''' <summary>
    ''' The mandant number.
    ''' </summary>
    Private m_MDNr As Integer

    ''' <summary>
    ''' The customer number.
    ''' </summary>
    Private m_KDNr As Integer

    ''' <summary>
    ''' The responsible person number.
    ''' </summary>
    Private m_ZHD As Integer

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="usNr">The The user number.</param>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="kdNr">The customer number.</param>
    ''' <param name="zhdNr">The ZHD number.</param>
    Public Sub New(ByVal sender As Object,
                   ByVal usNr As Integer,
                   ByVal mdNr As Integer,
                   ByVal kdNr As Integer,
                   ByVal zhdNr As Integer)
      MyBase.New(sender)

      m_USNr = usNr
      m_MDNr = mdNr
      m_KDNr = kdNr
      m_ZHD = zhdNr

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the user number.
    ''' </summary>
    ''' <returns>The user number.</returns>
    Public ReadOnly Property USNr As Integer
      Get
        Return m_USNr
      End Get
    End Property

    ''' <summary>
    ''' Gets the mandant number.
    ''' </summary>
    ''' <returns>The mandant number.</returns>
    Public ReadOnly Property MDNr As Integer
      Get
        Return m_MDNr
      End Get
    End Property

    ''' <summary>
    ''' Gets the KDNr number.
    ''' </summary>
    ''' <returns>The customer number</returns>
    Public ReadOnly Property KDNr As Integer
      Get
        Return m_KDNr
      End Get
    End Property

    ''' <summary>
    ''' Gets the ZHD number.
    ''' </summary>
    ''' <returns>The employee number</returns>
    Public ReadOnly Property ZHDNumber As Integer
      Get
        Return m_ZHD
      End Get
    End Property

#End Region

  End Class
End Namespace
