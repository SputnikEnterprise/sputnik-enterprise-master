Imports TinyMessenger

''' <summary>
''' Request to open empoyee contact.
''' </summary>
Public Class OpenMAKontaktMngRequest
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
  Private m_EmployeeNumber As Integer

  ''' <summary>
  ''' The contact record number.
  ''' </summary>
  Private m_ContactRecordNumber As Integer

#End Region

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  ''' <param name="sender">The sender.</param>
  ''' <param name="usNr">The The user number.</param>
  ''' <param name="mdNr">The mandant number.</param>
  ''' <param name="employeeNumber">The employee number.</param>
  ''' <param name="contactRecNumber">The contact record number.</param>
  Public Sub New(ByVal sender As Object,
                 ByVal usNr As Integer,
                 ByVal mdNr As Integer,
                 ByVal employeeNumber As Integer,
                 ByVal contactRecNumber As Integer)
    MyBase.New(sender)

    m_USNr = usNr
    m_MDNr = mdNr
    m_EmployeeNumber = employeeNumber
    m_ContactRecordNumber = contactRecNumber

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
  ''' <returns>The record number</returns>
  Public ReadOnly Property EmployeeNumber As Integer
    Get
      Return m_EmployeeNumber
    End Get
  End Property

  ''' <summary>
  ''' Gets the contact record number.
  ''' </summary>
  ''' <returns>The record number</returns>
  Public ReadOnly Property ContactRecordNumber As Integer
    Get
      Return m_ContactRecordNumber
    End Get
  End Property

#End Region

End Class
