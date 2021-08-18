
Imports SP.DatabaseAccess

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports DevExpress.LookAndFeel
Imports SPS.DTAUtility.Settings
Imports System.Reflection.Assembly
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports System.ComponentModel
Imports System.Linq
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel.Design
Imports DevExpress.XtraReports.UserDesigner
'Imports System.Data



Public Class xrpDTAList


#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_initData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The current connection string.
	''' </summary>
	Private m_CurrentConnectionString = String.Empty

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As SP.DatabaseAccess.Common.ICommonDatabaseAccess

	''' <summary>
	''' The DAT utility data access object.
	''' </summary>
	Private m_DTAUtilityDatabaseAccess As SP.DatabaseAccess.DTAUtility.IDTAUtilityDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Boolean flag indicating if initial data has been loaded.
	''' </summary>
	Private m_IsInitialDataLoaded As Boolean = False

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	Private m_IsDataValid As Boolean = True

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private m_md As Mandant
	Private m_path As ClsProgPath

	Private m_selectionColumnEdit As RepositoryItemCheckEdit

	Private m_mandantDataList As IEnumerable(Of MandantData)
	Private m_mandantDataSelected As MandantData

	Private m_jobNumberDataList As IList(Of SP.DatabaseAccess.DTAUtility.DataObjects.JobNumberData)
	Private m_jobNumberDataSelected As SP.DatabaseAccess.DTAUtility.DataObjects.JobNumberData

	Private m_bankDataList As IList(Of SP.DatabaseAccess.DTAUtility.DataObjects.BankData)
	Private m_bankDataSelected As SP.DatabaseAccess.DTAUtility.DataObjects.BankData


	Private m_selectionFormatString As String
	Private m_selectionPaymentsFormatString As String
	Private m_selectionPaymentsCreditorFormatString As String
	Private m_selectionJobsFormatString As String
	Private m_SelectedAmount As Decimal
	Private m_dtaDataList As List(Of SP.DatabaseAccess.DTAUtility.DataObjects.DtaDataForListing)


#End Region	' Private Fields

	Public Sub New(ByVal dtaDataList As List(Of SP.DatabaseAccess.DTAUtility.DataObjects.DtaDataForListing))

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		'Dim dtaDataList As List(Of SP.DatabaseAccess.DTAUtility.DataObjects.DtaDataForListing) = m_DTAUtilityDatabaseAccess.LoadZGADataForDTAList(m_mandantDataSelected.MandantNumber, dtaNumber)
		m_dtaDataList = dtaDataList
		'Me.DataSource = dtaDataList


		'Dim dt As New DataTable()
		'Dim rand As New Random()

		'For Each test In dtaDataList
		'	dt.Columns.Add(test.Betrag, GetType(String))
		'Next

		'For i As Integer = 0 To 9
		'	dt.Columns.Add("TestCol" & i.ToString(), GetType(Double))
		'Next i

		'dt.Columns.Add("ZGNr", GetType(Integer))
		'dt.Columns.Add("Nachname", GetType(String))
		'dt.Columns.Add("Vorname", GetType(String))
		'dt.Columns.Add("IBANNr", GetType(String))
		'dt.Columns.Add("Betrag", GetType(Decimal))
		'dt.Columns.Add("Bank", GetType(String))

		'For i As Integer = 0 To 7999
		'	dt.Rows.Add(New Object() {rand.NextDouble() * 1000, rand.NextDouble() * 1000, rand.NextDouble() * 1000, rand.NextDouble() * 1000})
		'	', rand.NextDouble() * 1000, rand.NextDouble() * 1000, rand.NextDouble() * 1000, rand.NextDouble() * 1000, rand.NextDouble() * 1000, rand.NextDouble() * 1000})
		'Next i

		Me.DataSource = dtaDataList
		'Me.CreateDocument(False)

		Dim rpt As New DevExpress.XtraReports.UI.ReportPrintTool(Me)
		rpt.ShowPreviewDialog()


	End Sub

	'Private Sub xrpDTAList_BeforePrint(sender As Object, e As Printing.PrintEventArgs) Handles Me.BeforePrint

	'	For Each test In m_dtaDataList
	'		Me.zgnr.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", test, "ZGNr")})
	'		Me.MAData.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", test, "Nachname")})
	'		Me.BCNr.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", test, "ClearingNr")})

	'		Me.Bank.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", test, "Bank")})
	'		Me.IBANNr.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", test, "IBANNr")})
	'		Me.ZGGrund.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", test, "ZGGrund")})
	'		Me.Monat.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", test, "LP")})

	'		Me.Betrag.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", test, "Betrag")})

	'	Next


	'End Sub

End Class