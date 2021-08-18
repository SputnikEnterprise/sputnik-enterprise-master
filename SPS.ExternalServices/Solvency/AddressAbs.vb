Imports SPS.ExternalServices.DeltavistaWebService

''' <summary>
''' Abstract base class of addrss data.
''' </summary>
Public MustInherit Class AddressAbs

    ''' <summary>
    ''' The address identifier.
    ''' </summary>
    Public Property AddressIdentifier As Identifier

    ''' <summary>
    ''' Gets the Deltavista address description from the address.
    ''' </summary>
    ''' <returns>The Deltavista address description.</returns>
    Public MustOverride ReadOnly Property AddressDescription As AddressDescription
End Class
