Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient
Imports SPS.Listing.Print.Utility
Imports SPS.Listing.Print.Utility.ESVerleih
Imports SPS.Listing.Print.Utility.ESVertrag
Imports SPS.Listing.Print.Utility.GAVStringData
Imports System.IO
Imports System.Text
Imports SP.DatabaseAccess.Listing
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing.DataObjects
Imports System.Reflection

Public Class ClsESListUtility


#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


	''' <summary>
	''' The Listing data access object.
	''' </summary>
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess


	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private m_Utility As SPProgUtility.MainUtilities.Utilities

#End Region


#Region "public properties"

	Public Property PControlSetting As New ClsParifondSetting
	Public Property m_TableName As String

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_Utility = New SPProgUtility.MainUtilities.Utilities
		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_InitializationData = _setting

		m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)


	End Sub

#End Region


	Private Function LoadESListData() As IEnumerable(Of ListingESListData)


		Dim listingDataList = m_ListingDatabaseAccess.LoadESListData(m_TableName, PControlSetting.liRPNr2Print)

		If (listingDataList Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatz-Daten konnten nicht geladen werden."))

			Return Nothing
		End If


		Return (listingDataList)

	End Function

	Public Function CreateESListToCsv(ByVal fileName As String, ByVal includeHeaderAsFirstRow As Boolean) As Boolean
		Dim success As Boolean = True
		Dim Separator As String = ";"
		Dim streamWriter As StreamWriter = New StreamWriter(fileName, False, System.Text.Encoding.Default)
		Dim sb As StringBuilder = Nothing
		Dim header As String = String.Join(Separator, "Einsatz-Nr.",
																			 "Sozialvers.-Nr.",
																			 "Mitarbeiter Name",
																			 "Geb.-Dat.",
																			 "erlernter Beruf",
																			 "Filiale",
																			 "Name Einsatzbetrieb",
																			 "UID",
																			 "Noga-Code",
																			 "Ort des Einsatzbetriebs",
																			 "Ort des Einsatzes",
																			 "Eins.-Beginn",
																			 "Eins.-Ende",
																			 "Hinterlegter GAV",
																			 "Einsatzfunktion",
																			 "Einstufung",
																			 "Erfahrungsjahre",
																			 "Grundlohn CHF",
																			 "Ferien CHF",
																			 "Ferien %",
																			 "Feiertage CHF",
																			 "Feiertage %",
																			 "13. ML CHF",
																			 "13. ML %",
																			 "Bruttolohn CHF",
																			 "Totalbetrag ausbezahlte Spesen, nach Spesenart",
																			 "Kumulierte Stunden Einsatz")

		Dim data = LoadESListData()
		If (data Is Nothing) OrElse data.Count = 0 Then Return False

		If includeHeaderAsFirstRow Then
			sb = New StringBuilder
			sb.Append(header)

			streamWriter.WriteLine(sb.ToString)
		End If

		Dim index As Integer = 0
		For Each itm In data
			sb = New StringBuilder

			Dim paresedGAVStirngData As New GAVStringData()
			paresedGAVStirngData.FillFromString(itm.GAVInfo_String)

			Dim esGAVValues = String.Join("- ",
																		m_Utility.ReplaceMissing(paresedGAVStirngData.Kanton, "???"),
																		m_Utility.ReplaceMissing(paresedGAVStirngData.Gruppe1, "???").Replace(";", "-"),
																		m_Utility.ReplaceMissing(paresedGAVStirngData.Gruppe2, "???").Replace(";", "-"),
																		m_Utility.ReplaceMissing(paresedGAVStirngData.Gruppe3, "???").Replace(";", "-"))


			Dim value As String = String.Join(Separator,
																				itm.ESNr.ToString, itm.ahv_nr, itm.employeeFullname.Replace(";", "-"),
																				If(itm.GebDat.HasValue, Format(itm.GebDat, "d"), String.Empty),
																				itm.MABeruf.Replace(";", "-"), itm.Filiale,
																				itm.customername.Replace(";", "-"), itm.UID, itm.NogaCode, itm.KDOrt.Replace(";", "-"),
																				itm.ESOrt.Replace(";", "-"),
																				If(itm.ES_Ab.HasValue, Format(itm.ES_Ab, "d"), String.Empty),
																				If(itm.ES_Ende.HasValue, Format(itm.ES_Ende, "d"), String.Empty),
																				itm.GAVGruppe0.Replace(";", "-"), itm.ES_Als, itm.GAVBezeichnung.Replace(";", "-"), esGAVValues,
																				itm.Grundlohn,
																				itm.Ferien, itm.FerienProz,
																				itm.Feier, itm.FeierProz,
																				itm.Lohn13, itm.Lohn13Proz,
																				itm.Bruttolohn,
																				itm.ESSpesen, itm.ESStunden)

			sb.Append(value)
			index += 1
			Trace.WriteLine(index)

			streamWriter.WriteLine(sb.ToString)
		Next

		streamWriter.Close()


		Return success

	End Function

	Private Function GetAllItems(cars As IEnumerable(Of ListingESListData)) As IEnumerable(Of String)
		Dim result As New List(Of String)()
		For Each i In cars
			result.AddRange(cars.Select(Function(x) x.ToString))
		Next

		Return result
	End Function



End Class

