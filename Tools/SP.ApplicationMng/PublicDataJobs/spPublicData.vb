
Imports System.IO
Imports System.Xml
Imports SP.DatabaseAccess.SPPublicDataJob
Imports SP.DatabaseAccess.SPPublicDataJob.DataObjects

Public Class spPublicData


	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_PublicDataDatabaseAccess As ISPPublicDataJobDatabaseAccess

	''' <summary>
	''' connections
	''' </summary>
	Private m_connStr_Application As String
	Private m_connStr_CVlizer As String
	Private m_connStr_Systeminfo As String
	Private m_connStr_Scanjobs As String
	Private m_connStr_Email As String
	Private m_connStr_PublicData As String


	Private m_SettingFile As ProgramSettings

	Private m_XmlFilename As String


	Public Sub New(ByVal settingFile As ProgramSettings, ByVal filename As String)

		m_XmlFilename = filename
		m_SettingFile = settingFile


		m_connStr_Application = m_SettingFile.ConnstringApplication
		m_connStr_CVlizer = m_SettingFile.ConnstringCVLizer
		m_connStr_Systeminfo = m_SettingFile.ConnstringSysteminfo
		m_connStr_Scanjobs = m_SettingFile.ConnstringScanjobs
		m_connStr_Email = m_SettingFile.ConnstringEMail
		m_connStr_PublicData = m_SettingFile.ConnstringSPPublicData


		m_PublicDataDatabaseAccess = New SPPublicDataJobDatabaseAccess(m_connStr_PublicData, "DE")


	End Sub

	Public Function ReadMunicipalitiesDataFromXMLFile() As Boolean
		Dim success As Boolean = True

		Dim i As Integer = 0
		m_XmlFilename = Path.Combine("<your path>", "your file.xml")

		Dim wcCantons As New List(Of CantonData)
		Dim wcDistricts As New List(Of DistrictData)
		Dim wcMunicipalities As New List(Of MunicipalityData)


		Dim reader As XmlReader = XmlReader.Create(m_XmlFilename)
		Try

			i = 0
			While (Not reader.EOF)

				If reader.Name <> "canton" Then
					reader.ReadToFollowing("canton")
				End If

				If Not reader.EOF Then
					Dim xCanton As XElement = XElement.ReadFrom(reader)
					Dim ns As XNamespace = xCanton.GetDefaultNamespace()
					Dim canton As New CantonData

					If Not xCanton.Element(ns + "cantonId") Is Nothing Then canton.CantonId = CType(xCanton.Element(ns + "cantonId"), Integer)
					If Not xCanton.Element(ns + "cantonAbbreviation") Is Nothing Then canton.CantonAbbreviation = xCanton.Element(ns + "cantonAbbreviation")
					If Not xCanton.Element(ns + "cantonLongName") Is Nothing Then canton.CantonLongName = xCanton.Element(ns + "cantonLongName")
					If Not xCanton.Element(ns + "cantonDateOfChange") Is Nothing Then canton.CantonDateOfChange = CType(xCanton.Element(ns + "cantonDateOfChange"), DateTime)

					wcCantons.Add(canton)

				End If

			End While

			For Each itm In wcCantons
				success = success AndAlso m_PublicDataDatabaseAccess.AddCantonData(itm)
			Next
			success = True


			reader = XmlReader.Create(m_XmlFilename)
			While (Not reader.EOF)

				If reader.Name <> "district" Then
					reader.ReadToFollowing("district")
				End If

				If Not reader.EOF Then
					Dim xDistrict As XElement = XElement.ReadFrom(reader)
					Dim ns As XNamespace = xDistrict.GetDefaultNamespace()
					Dim district As New DistrictData

					If Not xDistrict.Element(ns + "districtHistId") Is Nothing Then district.DistrictHistId = CType(xDistrict.Element(ns + "districtHistId"), Integer)
					If Not xDistrict.Element(ns + "cantonId") Is Nothing Then district.CantonId = CType(xDistrict.Element(ns + "cantonId"), Integer)
					If Not xDistrict.Element(ns + "districtId") Is Nothing Then district.DistrictId = CType(xDistrict.Element(ns + "districtId"), Integer)
					If Not xDistrict.Element(ns + "districtLongName") Is Nothing Then district.DistrictLongName = xDistrict.Element(ns + "districtLongName")
					If Not xDistrict.Element(ns + "districtShortName") Is Nothing Then district.DistrictShortName = xDistrict.Element(ns + "districtShortName")
					If Not xDistrict.Element(ns + "districtEntryMode") Is Nothing Then district.DistrictEntryMode = CType(xDistrict.Element(ns + "districtEntryMode"), Integer)
					If Not xDistrict.Element(ns + "districtAdmissionNumber") Is Nothing Then district.DistrictAdmissionNumber = CType(xDistrict.Element(ns + "districtAdmissionNumber"), Integer)
					If Not xDistrict.Element(ns + "districtAdmissionMode") Is Nothing Then district.DistrictAdmissionMode = CType(xDistrict.Element(ns + "districtAdmissionMode"), Integer)
					If Not xDistrict.Element(ns + "districtAdmissionDate") Is Nothing Then district.DistrictAdmissionDate = CType(xDistrict.Element(ns + "districtAdmissionDate"), DateTime)
					If Not xDistrict.Element(ns + "districtAbolitionNumber") Is Nothing Then district.DistrictAbolitionNumber = CType(xDistrict.Element(ns + "districtAbolitionNumber"), Integer)
					If Not xDistrict.Element(ns + "districtAbolitionMode") Is Nothing Then district.DistrictAbolitionMode = CType(xDistrict.Element(ns + "districtAbolitionMode"), Integer)
					If Not xDistrict.Element(ns + "districtAbolitionDate") Is Nothing Then district.DistrictAbolitionDate = CType(xDistrict.Element(ns + "districtAbolitionDate"), DateTime)
					If Not xDistrict.Element(ns + "districtDateOfChange") Is Nothing Then district.DistrictDateOfChange = CType(xDistrict.Element(ns + "districtDateOfChange"), DateTime)


					wcDistricts.Add(district)

				End If

			End While

			For Each itm In wcDistricts
				success = success AndAlso m_PublicDataDatabaseAccess.AddDistrictData(itm)
			Next
			success = True


			reader = XmlReader.Create(m_XmlFilename)
			While (Not reader.EOF)

				If reader.Name <> "municipality" Then
					reader.ReadToFollowing("municipality")
				End If

				If Not reader.EOF Then
					Dim xMunicipality As XElement = XElement.ReadFrom(reader)
					Dim ns As XNamespace = xMunicipality.GetDefaultNamespace()
					Dim municipality As New MunicipalityData

					If Not xMunicipality.Element(ns + "historyMunicipalityId") Is Nothing Then municipality.HistoryMunicipalityId = CType(xMunicipality.Element(ns + "historyMunicipalityId"), Integer)
					If Not xMunicipality.Element(ns + "districtHistId") Is Nothing Then municipality.DistrictHistId = CType(xMunicipality.Element(ns + "districtHistId"), Integer)
					If Not xMunicipality.Element(ns + "cantonAbbreviation") Is Nothing Then municipality.CantonAbbreviation = xMunicipality.Element(ns + "cantonAbbreviation")
					If Not xMunicipality.Element(ns + "municipalityId") Is Nothing Then municipality.MunicipalityId = CType(xMunicipality.Element(ns + "municipalityId"), Integer)
					If Not xMunicipality.Element(ns + "municipalityShortName") Is Nothing Then municipality.MunicipalityShortName = xMunicipality.Element(ns + "municipalityShortName")
					If Not xMunicipality.Element(ns + "municipalityLongName") Is Nothing Then municipality.MunicipalityLongName = xMunicipality.Element(ns + "municipalityLongName")
					If Not xMunicipality.Element(ns + "municipalityEntryMode") Is Nothing Then municipality.MunicipalityEntryMode = CType(xMunicipality.Element(ns + "municipalityEntryMode"), Integer)
					If Not xMunicipality.Element(ns + "municipalityStatus") Is Nothing Then municipality.MunicipalityStatus = CType(xMunicipality.Element(ns + "municipalityStatus"), Integer)
					If Not xMunicipality.Element(ns + "municipalityAdmissionNumber") Is Nothing Then municipality.MunicipalityAdmissionNumber = CType(xMunicipality.Element(ns + "municipalityAdmissionNumber"), Integer)
					If Not xMunicipality.Element(ns + "municipalityAdmissionMode") Is Nothing Then municipality.MunicipalityAdmissionMode = CType(xMunicipality.Element(ns + "municipalityAdmissionMode"), Integer)
					If Not xMunicipality.Element(ns + "municipalityAdmissionDate") Is Nothing Then municipality.MunicipalityAdmissionDate = CType(xMunicipality.Element(ns + "municipalityAdmissionDate"), DateTime)
					If Not xMunicipality.Element(ns + "municipalityAbolitionNumber") Is Nothing Then municipality.MunicipalityAbolitionNumber = CType(xMunicipality.Element(ns + "municipalityAbolitionNumber"), Integer)
					If Not xMunicipality.Element(ns + "municipalityAbolitionMode") Is Nothing Then municipality.MunicipalityAbolitionMode = CType(xMunicipality.Element(ns + "municipalityAbolitionMode"), Integer)
					If Not xMunicipality.Element(ns + "municipalityAbolitionDate") Is Nothing Then municipality.MunicipalityAbolitionDate = CType(xMunicipality.Element(ns + "municipalityAbolitionDate"), DateTime)
					If Not xMunicipality.Element(ns + "municipalityDateOfChange") Is Nothing Then municipality.MunicipalityDateOfChange = CType(xMunicipality.Element(ns + "municipalityDateOfChange"), DateTime)


					wcMunicipalities.Add(municipality)

				End If

			End While

			For Each itm In wcMunicipalities
				success = success AndAlso m_PublicDataDatabaseAccess.AddMunicipalityData(itm)
			Next
			success = True



		Catch ex As Exception

		End Try

	End Function

End Class
