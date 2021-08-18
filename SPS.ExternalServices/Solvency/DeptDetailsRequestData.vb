Imports SPS.ExternalServices.DeltavistaWebService

''' <summary>
''' Dept details request data.
''' </summary>
Public Class DeptDetailsRequestData

#Region "Private Fields"

    Private m_Identifier As Identifier
    Private m_ReferenceNumber As String

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="identifer">The identifier.</param>
    ''' <param name="referenceNumber">The reference number.</param>
    Public Sub New(ByVal identifer As Identifier, ByVal referenceNumber As String)

        If (identifer Is Nothing) Then
            Throw New ArgumentException("Identifier must be provided.")
        End If

        If (String.IsNullOrEmpty(referenceNumber)) Then
            Throw New ArgumentException("Refrence number must be provided.")
        End If

        m_Identifier = identifer
        m_ReferenceNumber = referenceNumber

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the identifier.
    ''' </summary>
    ''' <returns>The identifier.</returns>
    Public ReadOnly Property Identifer As Identifier
        Get
            Return m_Identifier
        End Get
    End Property

    ''' <summary>
    ''' Gets the reference number.
    ''' </summary>
    ''' <returns>The reference number.</returns>
    Public ReadOnly Property ReferenceNumber As String
        Get
            Return m_ReferenceNumber
        End Get
    End Property


#End Region

End Class
