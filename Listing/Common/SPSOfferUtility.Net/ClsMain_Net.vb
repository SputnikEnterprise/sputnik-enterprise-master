
Imports System.IO.File
Imports System.IO
Imports SPProgUtility

Imports SP.Infrastructure.UI


Public Class ClsMain_Net

	'Implements IDisposable
	'Protected disposed As Boolean = False

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		ClsOfDetails.m_InitialData = _setting
		ClsOfDetails.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_UtilityUI = New UtilityUI
		m_Translate = ClsOfDetails.m_Translate

		Application.EnableVisualStyles()

	End Sub

	Public Sub New()
		Dim m_md As New SPProgUtility.Mandanten.Mandant

		Dim _setting As SP.Infrastructure.Initialization.InitializeClass = CreateInitialData(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr)
		ClsOfDetails.m_InitialData = _setting
		ClsOfDetails.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_UtilityUI = New UtilityUI
		m_Translate = ClsOfDetails.m_Translate

		Application.EnableVisualStyles()

	End Sub

	'Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
	'	If Not Me.disposed Then
	'		If disposing Then

	'		End If
	'		' Add code here to release the unmanaged resource.
	'		'LL.Dispose()
	'		'LL.Core.Dispose()
	'		' Note that this is not thread safe.
	'	End If
	'	Me.disposed = True
	'End Sub

	'#Region " IDisposable Support "
	'	' Do not change or add Overridable to these methods.
	'	' Put cleanup code in Dispose(ByVal disposing As Boolean).
	'	Public Overloads Sub Dispose() Implements IDisposable.Dispose
	'		Dispose(True)
	'		GC.SuppressFinalize(Me)
	'	End Sub
	'	Protected Overrides Sub Finalize()
	'		Dispose(False)
	'		MyBase.Finalize()
	'	End Sub
	'#End Region


#End Region


	Function ShowMainForm(ByVal strQuery As String) As Boolean
		Dim frmTest As New frmOfferSelect(ClsOfDetails.m_InitialData)
		Dim strTestSql As String = String.Empty

		Dim strBeginTrySql As String
		strBeginTrySql = "BEGIN TRY DROP TABLE #KD_Mailing END TRY BEGIN CATCH END CATCH"

		strTestSql = String.Format("{0} SELECT * {1} FROM _Kundenliste_{2} ",
															 strBeginTrySql,
															 "Into #KD_Mailing",
															 ClsOfDetails.m_InitialData.UserData.UserNr)
		strTestSql &= "Declare @Anzrec int "
		strTestSql &= "Set @Anzrec = (select COUNT(*) As Anz from #KD_Mailing) "
		strTestSql &= "Select @Anzrec As Anzrec, * From #KD_Mailing"

		'ClsOfDetails.GetOrgProgQuery = strTestSql

		frmTest.m_GetSearchQuery = strTestSql
		If frmTest.LoadData() Then
			frmTest.Show()
			frmTest.BringToFront()

		Else
			frmTest = Nothing

		End If

		'Finalize()

		Return True
	End Function

	Function PrintLLDoc(ByVal iSelectedOfNr As Integer, ByVal iMANr As Integer,
											ByVal iKDNr As Integer, ByVal iZHDNr As Integer,
											ByVal bShowForm As Boolean,
											ByVal bForExport As Boolean, ByVal strModulToPrint As String,
											Optional ByVal strQueryString As String = "") As Boolean
		Dim success As Boolean = True

		Dim _setting As New SPS.Listing.Print.Utility.ClsLLOfferSearchPrintSetting With {.m_initData = ClsOfDetails.m_InitialData,
																																								 .offerNumber = iSelectedOfNr,
																																								 .JobNr2Print = strModulToPrint,
																																								 .ShowAsExport = bForExport,
																																								 .ShowAsDesgin = False}

		If iKDNr = 0 Then
			Dim customerdata = GetCustomerForOfferData(ClsOfDetails.m_InitialData.MDData.MDDbConn, iSelectedOfNr)
			If customerdata Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Kundendaten wurden gefunden!"))

				Return False
			End If

			_setting.customerNumber = customerdata.CustomerNumber
			_setting.cresponsibleNumber = customerdata.CResponsibleNumber

		Else
			_setting.customerNumber = iKDNr
			_setting.cresponsibleNumber = iZHDNr

		End If

		Dim printTemplate As New SPS.Listing.Print.Utility.OfferSearchListing.ClsPrintOfferSearchList(_setting)
		Dim strOfferblattFileName As String = printTemplate.PrintOfferTemplate()

		If bForExport Then Process.Start(strOfferblattFileName)

		success = Not strOfferblattFileName.Contains("error")

		Return success
	End Function

	Function PrintLLDocToFile(ByVal iSelectedOfNr As Integer, ByVal iMANr As Integer,
														ByVal iKDNr As Integer, ByVal iZHDNr As Integer,
														ByVal bShowForm As Boolean,
														ByVal bForExport As Boolean, ByVal strModulToPrint As String) As String

		ClsOfDetails.GetSelectedKDNr = iKDNr
		ClsOfDetails.GetSelectedKDZNr = iZHDNr
		ClsOfDetails.GetSelectedMANr = iMANr
		ClsOfDetails.iOfNr = iSelectedOfNr

		Dim result As Boolean = True
		Dim strJobNr As String = strModulToPrint

		Dim _setting As New SPS.Listing.Print.Utility.ClsLLOfferSearchPrintSetting With {.m_initData = ClsOfDetails.m_InitialData,
																																										 .offerNumber = iSelectedOfNr,
																																										 .customerNumber = iKDNr,
																																										 .cresponsibleNumber = iZHDNr,
																																										 .JobNr2Print = strJobNr,
																																										 .ShowAsExport = True,
																																										 .ShowAsDesgin = False}

		Dim printTemplate As New SPS.Listing.Print.Utility.OfferSearchListing.ClsPrintOfferSearchList(_setting)
		Dim ExportedFilename = printTemplate.PrintOfferTemplate()

		If String.IsNullOrWhiteSpace(ExportedFilename) OrElse File.Exists(ExportedFilename) Then
			ExportedFilename = String.Empty
		End If

		Return ExportedFilename
	End Function



#Region "Helpers"

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant As ClsMDData = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData As ClsUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData As Dictionary(Of String, ClsProsonalizedData) = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate As Dictionary(Of String, ClsTranslationData) = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

#End Region

End Class
