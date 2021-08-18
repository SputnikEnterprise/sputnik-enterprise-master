Imports TinyMessenger

Namespace Messaging.Messages

  ''' <summary>
  ''' Request to open a monthly salary management form.
  ''' </summary>
  Public Class OpenMLohnMngRequest
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
    ''' The employee number.
    ''' </summary>
    Private m_MANr As Integer

    ''' <summary>
    ''' The monthly salary number.
    ''' </summary>
    Private m_LMNr As Integer

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="usNr">The The user number.</param>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="maNr">The employee number.</param>
    ''' <param name="lmNr">The monthly salary number.</param>
    Public Sub New(ByVal sender As Object,
                   ByVal usNr As Integer,
                   ByVal mdNr As Integer,
                   ByVal maNr As Integer,
                   ByVal lmNr As Integer)
      MyBase.New(sender)

      m_USNr = usNr
      m_MDNr = mdNr
      m_MANr = maNr
      m_LMNr = lmNr

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
    ''' Gets the employee number.
    ''' </summary>
    ''' <returns>The employee number.</returns>
    Public ReadOnly Property MANr As Integer
      Get
        Return m_MANr
      End Get
    End Property

    ''' <summary>
    ''' Gets the monthly salary number.
    ''' </summary>
    ''' <returns>The employee number.</returns>
    Public ReadOnly Property LMNr As Integer
      Get
        Return m_LMNr
      End Get
    End Property

#End Region

  End Class
End Namespace


