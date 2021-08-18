Imports System.ComponentModel
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Internal.Automations.SPApplicationWebService
Imports SP.Internal.Automations.SPNotificationWebService


Namespace BaseTable




	Partial Class SPSBaseTables


		Private m_InvalidData As List(Of String)

		Public Function UpdateEmployeeGeoData(ByVal countryCode As String) As Boolean
			Dim success As Boolean = True

			Dim listDataSource As BindingList(Of LocationGoordinateViewData) = New BindingList(Of LocationGoordinateViewData)

			listDataSource = PerformGeoCoordinationDatalistWebserviceCall(countryCode)
			If listDataSource Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Geo-Koordinaten konnten nicht geladen werden."))

				Return False
			End If
			BaseTableName = "Country"
			Dim countryData = PerformCVLBaseTablelistWebserviceCall()
			If countryData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Länderdaten konnten nicht geladen werden."))

				Return False
			End If


			Dim employeeData = m_ListingDatabaseAccess.LoadAllEmployeeMasterData(False)
			If employeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Mandant: {0} >>> Kandidatendaten konnten nicht geladen werden.", m_InitializationData.MDData.MDNr))

				Return False
			End If
			employeeData = employeeData.Where(Function(x) x.Country = countryCode).ToList()

			For Each employee In employeeData
				Dim country = countryData.Where(Function(x) x.Code = employee.Country).FirstOrDefault()
				If Not country Is Nothing Then
					Dim geoData = listDataSource.Where(Function(x) x.Postcode = Trim(employee.Postcode) And x.CountryCode = employee.Country).FirstOrDefault()

					If Not geoData Is Nothing Then

						If employee.Latitude.GetValueOrDefault(0) = 0 OrElse employee.Longitude.GetValueOrDefault(0) = 0 Then
							employee.Latitude = geoData.Latitude
							employee.Longitude = geoData.Longitude

							success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeGeoData(employee)
						End If
					Else
						m_Logger.LogWarning(String.Format("geo data could not be founded! {0}: '{1}' >>> '{2}'", employee.EmployeeNumber, employee.Postcode, employee.Country))

						Trace.WriteLine(String.Format("geo data could not be founded! {0}: {2}-{1}", employee.EmployeeNumber, employee.Postcode, employee.Country))

					End If

					If Not success Then Exit For
				End If
			Next

			Return success

		End Function

		Public Function UpdateEmployeeCountryData() As Boolean
			Dim success As Boolean = True

			BaseTableName = "Country"
			Dim countryData = PerformCVLBaseTablelistWebserviceCall()
			Dim existingCountryData = m_CommonDatabaseAccess.LoadCountryData()
			If countryData Is Nothing OrElse existingCountryData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Länderdaten konnten nicht geladen werden."))

				Return False
			End If

			Dim employeeData = m_ListingDatabaseAccess.LoadAllEmployeeCountryCodeData()
			If employeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Mandant: {0} >>> Kandidatendaten konnten nicht geladen werden.", m_InitializationData.MDData.MDNr))

				Return False
			End If

			For Each employee In employeeData
				Dim country = countryData.Where(Function(x) x.Code = employee.Country).FirstOrDefault()
				If country Is Nothing Then
					Dim oldcountry = existingCountryData.Where(Function(x) x.Code = employee.Country).FirstOrDefault()

					If Not oldcountry Is Nothing Then
						country = countryData.Where(Function(x) x.Translated_Value = oldcountry.Name).FirstOrDefault()
						If Not country Is Nothing Then
							success = success AndAlso m_ListingDatabaseAccess.UpdateEmployeeCountryData(employee.Country, country.Code)
						End If
					End If

				End If

				If Not success Then Exit For
			Next

			success = True
			employeeData = m_ListingDatabaseAccess.LoadAllEmployeeNationalityCodeData()
			If employeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Mandant: {0} >>> Kandidatendaten konnten nicht geladen werden.", m_InitializationData.MDData.MDNr))

				Return False
			End If

			For Each employee In employeeData
				Dim country = countryData.Where(Function(x) x.Code = employee.Nationality).FirstOrDefault()

				If country Is Nothing Then
					Dim oldcountry = existingCountryData.Where(Function(x) x.Code = employee.Nationality).FirstOrDefault()
					If Not oldcountry Is Nothing Then
						country = countryData.Where(Function(x) x.Translated_Value = oldcountry.Name).FirstOrDefault()
						If Not country Is Nothing Then
							success = success AndAlso m_ListingDatabaseAccess.UpdateEmployeeNationalityData(employee.Nationality, country.Code)
						End If
					End If

				End If

				If Not success Then Exit For
			Next


			Return success

		End Function

		Public Function UpdateEmployeeTaxCommunityData() As Boolean
			Dim success As Boolean = True
			m_InvalidData = New List(Of String)

			Dim employeeData = m_ListingDatabaseAccess.LoadAllEmployeeCommunityCodeData()
			If employeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Kandidatendaten konnten nicht geladen werden."))

				Return False
			End If
			Dim communityData = PerformCommunityDataOverWebService(String.Empty, m_InitializationData.UserData.UserLanguage)

			For Each employee In employeeData
				Dim community = communityData.Where(Function(x) x.BFSNumber = employee.TaxCommunityCode).FirstOrDefault()
				If m_InvalidData.Count = 0 Then m_InvalidData.Add(String.Format("<br><b>Suche nach entsprechenden Gemeindebezeichnungen:</b>"))
				If Not community Is Nothing Then
					m_InvalidData.Add(String.Format("Neu: {0} | Gemeinde: ({1}) {2}", community.Canton, employee.TaxCommunityCode, community.Translated_Value))
					employee.S_Canton = community.Canton
					employee.TaxCommunityLabel = community.Translated_Value

					success = success AndAlso m_ListingDatabaseAccess.UpdateEmployeeCommunityData(employee)
				Else

					m_InvalidData.Add(String.Format("({0}) Nicht gefunden!", employee.TaxCommunityCode))

				End If

				If Not success Then Exit For
			Next
			success = success AndAlso UpdateUndifinedEmployeeTaxCommunityData()

			If m_InvalidData.Count > 0 Then
				m_UtilityUI.SendMailNotification("Fehlerhafte Quellensteuer-Gemeinden und Quellensteuer-Kantone",
												 String.Format("Mandant: {0} >>> {1}<br>Folgende Codes sind felderhaft. Bitte korrigieren Sie die Daten.<br>{2}",
															   m_InitializationData.MDData.MDGuid, m_InitializationData.MDData.MDName, String.Join("<br>", m_InvalidData)),
												 String.Empty,
												 Nothing)
				success = False
			End If

			Return success

		End Function

		Private Function UpdateUndifinedEmployeeTaxCommunityData() As Boolean
			Dim success As Boolean = True
			Dim i As Integer = 0

			Dim employeeData = m_ListingDatabaseAccess.LoadAllUnDefinedEmployeeCommunityLabelData()
			If employeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Kandidatendaten konnten nicht geladen werden."))

				Return False
			End If
			Dim communityData = PerformCommunityDataOverWebService(String.Empty, m_InitializationData.UserData.UserLanguage)

			For Each employee In employeeData
				Dim community = communityData.Where(Function(x) x.Translated_Value = employee.TaxCommunityLabel).FirstOrDefault()
				If i = 0 Then m_InvalidData.Add(String.Format("<br><b>Suche nach entsprechenden Gemeindecode:</b>"))
				If Not community Is Nothing Then
					m_InvalidData.Add(String.Format("Neu: {0} | Gemeinde: ({1}) {2}", community.Canton, community.BFSNumber, employee.TaxCommunityLabel))
					employee.S_Canton = community.Canton
					employee.TaxCommunityCode = community.BFSNumber

					success = success AndAlso m_ListingDatabaseAccess.UpdateUnDefinedEmployeeCommunityData(employee)
				Else

					m_InvalidData.Add(String.Format("({0}) Nicht gefunden!", employee.TaxCommunityLabel))

				End If

				i += 1
				If Not success Then Exit For
			Next

			Return success

		End Function
	End Class


End Namespace
