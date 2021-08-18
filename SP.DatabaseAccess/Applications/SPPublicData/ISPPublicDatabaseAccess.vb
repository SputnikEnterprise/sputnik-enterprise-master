
Imports SP.DatabaseAccess.SPPublicDataJob.DataObjects


Namespace SPPublicDataJob


	''' <summary>
	''' Interface for ISPPublicJobDatabaseAccess database access.
	''' </summary>
	Public Interface ISPPublicDataJobDatabaseAccess

		Function AddCantonData(ByVal cantonData As CantonData) As Boolean
		Function AddDistrictData(ByVal districtData As DistrictData) As Boolean
		Function AddMunicipalityData(ByVal municipalityData As MunicipalityData) As Boolean


	End Interface

End Namespace
