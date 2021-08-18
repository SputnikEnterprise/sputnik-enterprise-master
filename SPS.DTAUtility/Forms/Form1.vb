' Developer Express Code Central Example:
' How to refresh the Field List in the End-User Designer
' 
' This example demonstrates how to programmatically update the Field List in the
' End-User Designer. This may be required when a data source is created and bound
' to a report at runtime, and isn't present in the designer host of the End-User
' Designer. For instance, this situation occurs when a data source is represented
' by a list (e.g. ArrayList), as demonstrated in the sample below.
' To accomplish
' this task, we've introduced the FieldListDockPanel.UpdateDataSource method,
' which should be called after assigning a data source to a report.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E1584

Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Windows.Forms
Imports System.ComponentModel.Design
Imports DevExpress.XtraBars
Imports DevExpress.XtraReports.UserDesigner
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Settings

' ...

Namespace FieldList_UpdateDataSource
	Partial Public Class Form1
		Inherits Form

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
			InitializeComponent()

			m_dtaDataList = dtaDataList
			BindReportToData()
		End Sub

		Private Const xmlPath As String = "..\..\Cars.xml"

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			xrDesignPanel1.ExecCommand(ReportCommand.NewReport)
		End Sub

		Private Sub commandBarItem46_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs) Handles barButtonItem1.ItemClick
			BindReportToData()
		End Sub

#Region "#UpdateFieldList"
		Private Sub BindReportToData()
			If xrDesignPanel1.Report Is Nothing Then
				Return
			End If
			' Create a data source and bind it to a report.
			xrDesignPanel1.Report.DataSource = m_dtaDataList

			' Update the Field List.
			Dim fieldList As FieldListDockPanel = CType(xrDesignDockManager1(DesignDockPanelType.FieldList), FieldListDockPanel)
			Dim host As IDesignerHost = CType(xrDesignPanel1.GetService(GetType(IDesignerHost)), IDesignerHost)
			fieldList.UpdateDataSource(host)
		End Sub
#End Region	' #UpdateFieldList


	End Class

End Namespace