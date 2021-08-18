Imports TinyMessenger

Namespace Messaging.Messages

  ''' <summary>
  ''' Notify changed payroll processing state..
  ''' </summary>
  Public Class PayrollProcessingStateHasChanged
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
    ''' Boolean flag indicating if payroll is in processing state.
    ''' </summary>
    Private m_IsPayrollProcessing As Integer
#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="usNr">The The user number.</param>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="isPayrollProcessing">The payroll processing state.</param>
    Public Sub New(ByVal sender As Object,
                   ByVal usNr As Integer,
                   ByVal mdNr As Integer,
                   ByVal isPayrollProcessing As Boolean)
      MyBase.New(sender)

      m_USNr = usNr
      m_MDNr = mdNr
      m_IsPayrollProcessing = isPayrollProcessing

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
    ''' Gets the payroll processing state.
    ''' </summary>
    ''' <returns>The payroll processing state.</returns>
    Public ReadOnly Property IsPayrollProcessing As Integer
      Get
        Return m_IsPayrollProcessing
      End Get
    End Property

#End Region

  End Class
End Namespace
