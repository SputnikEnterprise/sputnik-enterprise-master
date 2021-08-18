Imports SPS.ExternalServices.DeltavistaWebService

''' <summary>
''' Consumer address data.
''' </summary>
Public Class ConsumerAddress
    Inherits AddressAbs
#Region "Private Fields"

    Private m_ContactItems As New List(Of ContactItem)

#End Region

#Region "Public Properties"

    Public Property Name As String
    Public Property FirstName As String
    Public Property Street As String
    Public Property HouseNumber As String
    Public Property Zip As String
    Public Property City As String

    ''' <summary>
    ''' Country code in ISO-3166-1 2-alpha (e.g. CH) or ISO-3166-1 3-alpha (e.g. CHE) format.
    ''' </summary>
    Public Property CountryCode As String
    Public Property Birthdate As Date
    Public Property Sex As Sex

    ''' <summary>
    ''' Additional contact items.
    ''' </summary>
    Public ReadOnly Property ContactItems As ContactItem()
        Get
            Return m_ContactItems.ToArray()
        End Get
    End Property

    ''' <summary>
    ''' Gets the formated Birthdate.
    ''' </summary>
    ''' <value>The formatted birthdate.</value>
    Public ReadOnly Property FormatedBirthdate
        Get
            Return Birthdate.ToString("yyyy-MM-dd")
        End Get
    End Property

    ''' <summary>
    ''' Gets the Deltavista address description from the address.
    ''' </summary>
    ''' <returns>The Deltavista address description.</returns>
    Public Overrides ReadOnly Property AddressDescription As AddressDescription
        Get

            Dim locationInfo As New Location
            locationInfo.street = Street
            locationInfo.houseNumber = HouseNumber
            locationInfo.zip = Zip
            locationInfo.city = City
            locationInfo.country = CountryCode

            Dim searchedAddress As PersonAddressDescription = New PersonAddressDescription()
            searchedAddress.location = locationInfo
            searchedAddress.firstName = FirstName
            searchedAddress.lastName = Name
            searchedAddress.birthDate = FormatedBirthdate
            searchedAddress.sex = Sex

            searchedAddress.contactItems = ContactItems

            Return searchedAddress

        End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Adds a contact item.
    ''' </summary>
    ''' <param name="contactType">The contact type.</param>
    ''' <param name="contactText">The contact text.</param>
    Public Sub AddContItem(ByVal contactType As ContactType, ByVal contactText As String)

        Dim contactItem As New ContactItem
        contactItem.contactType = contactType
        contactItem.contactText = contactText

        m_ContactItems.Add(contactItem)
    End Sub

#End Region

End Class
