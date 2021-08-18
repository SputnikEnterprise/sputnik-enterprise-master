

Namespace SPPublicDataJob.DataObjects


	Public Class CantonData
		Public Property CantonId As Integer?
		Public Property CantonAbbreviation As String
		Public Property CantonLongName As String
		Public Property CantonDateOfChange As DateTime?

	End Class

	Public Class DistrictData
		Public Property DistrictHistId As Integer?
		Public Property CantonId As Integer?
		Public Property DistrictId As Integer?
		Public Property DistrictLongName As String
		Public Property DistrictShortName As String
		Public Property DistrictEntryMode As Integer?
		Public Property DistrictAdmissionNumber As Integer?
		Public Property DistrictAdmissionMode As Integer?
		Public Property DistrictAdmissionDate As DateTime?
		Public Property DistrictAbolitionNumber As Integer?
		Public Property DistrictAbolitionMode As Integer?
		Public Property DistrictAbolitionDate As DateTime?
		Public Property DistrictDateOfChange As DateTime?

	End Class

	Public Class MunicipalityData
		Public Property HistoryMunicipalityId As Integer?
		Public Property DistrictHistId As Integer?
		Public Property CantonAbbreviation As String
		Public Property MunicipalityId As Integer?
		Public Property MunicipalityLongName As String
		Public Property MunicipalityShortName As String
		Public Property MunicipalityEntryMode As Integer?
		Public Property MunicipalityStatus As Integer?
		Public Property MunicipalityAdmissionNumber As Integer?
		Public Property MunicipalityAdmissionMode As Integer?
		Public Property MunicipalityAdmissionDate As DateTime?
		Public Property MunicipalityAbolitionNumber As Integer?
		Public Property MunicipalityAbolitionMode As Integer?
		Public Property MunicipalityAbolitionDate As DateTime?
		Public Property MunicipalityDateOfChange As DateTime?

	End Class


End Namespace
