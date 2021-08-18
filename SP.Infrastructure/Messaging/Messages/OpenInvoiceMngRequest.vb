Imports TinyMessenger

Namespace Messaging.Messages

  ''' <summary>
  ''' Request to open an invoice management form.
  ''' </summary>
  Public Class OpenInvoiceMngRequest
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
    ''' The invoice number.
    ''' </summary>
    Private m_InvoiceNumber As Integer

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="usNr">The The user number.</param>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="invoiceNumber">The invoice number.</param>
    Public Sub New(ByVal sender As Object,
                   ByVal usNr As Integer,
                   ByVal mdNr As Integer,
                   ByVal invoiceNumber As Integer)
      MyBase.New(sender)

      m_USNr = usNr
      m_MDNr = mdNr
      m_InvoiceNumber = InvoiceNumber

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
    ''' Gets the invoice number.
    ''' </summary>
    ''' <returns>The invoice number</returns>
    Public ReadOnly Property InvoiceNumber As Integer
      Get
        Return m_InvoiceNumber
      End Get
    End Property

#End Region

  End Class
End Namespace
