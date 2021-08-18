

Imports System.Reflection.Assembly
Imports DevExpress.LookAndFeel
Imports System.Windows.Forms
Imports System.Threading
Imports System.IO

Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.CommonXmlUtility

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging

Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects


'Imports SP.MA.EinsatzMng.SPUpdateUtilitiesService
Imports SP.MA.ReportMng

Public Class ReportJobUtilities



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
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	Private m_connectionString As String
	'Private m_reportFinishFlagUpdate As ReportFinishedFlagUpdater

#Region "public properties"

	Public Property ChangeOwnReports As Boolean

#End Region

#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)


	End Sub

	Public Function UpdateReportFinishFlag()
		Dim success As Boolean = True

		Dim rpKST As String = m_InitializationData.UserData.UserKST

		If Not ChangeOwnReports Then rpKST = String.Empty
		Dim esData = m_ListingDatabaseAccess.LoadActiveReportData(m_InitializationData.MDData.MDNr, Now.Date, Now.Date, rpKST)

		For Each es In esData
			If es.RPNR = 40547 Then
				Trace.WriteLine(es.RPNR)
			End If

			success = success AndAlso CheckAssignedReportState(es.RPNR)   ' m_reportFinishFlagUpdate.UpdateFinishedFlagOfSingleReport(es.RPNR)
			success = success AndAlso m_ListingDatabaseAccess.AddReportFinishingFlagCheck(m_InitializationData.MDData.MDNr, es.ESNR, es.RPNR, m_InitializationData.UserData.UserFullName)

				m_Logger.LogDebug(String.Format("report number {0} for rpkst {1} were checked! {2}", es.RPNR, rpKST, success))
		Next


		Return success

	End Function

	Private Function CheckAssignedReportState(ByVal rpNumber As Integer)
		Dim success As Boolean = True

		Return success

	End Function

#End Region



End Class
