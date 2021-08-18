
Imports System.ComponentModel

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Configuration
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Diagnostics



Imports DevExpress.XtraEditors
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Initialization
Imports System.Threading.Tasks
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports System.Threading
'Imports SP.Main.Notify.SPNotificationWebService
Imports DevExpress.XtraEditors.Repository
Imports SP.Main.Notify.ScanJobs
Imports DevExpress.XtraGrid.Views.Base
Imports System.Management
Imports SP.DatabaseAccess.ScanJob.DataObjects
Imports System.IO
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.Internal.Automations.SPNotificationWebService
Imports DevExpress.XtraRichEdit
Imports DevExpress.Utils.About
Imports DevExpress.XtraEditors.ViewInfo
Imports SP.Internal.Automations

Namespace UI



	Public Class frmUpdateNotifications

#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx"
		Private Const DEFAULT_SPUTNIK_SCANJOB_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPScanJobUtility.asmx"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES As String = "MD_{0}/Sonstiges"

		Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
		Private Const PROGRAM_SETTING_FILE As String = "NotifyerSettings.xml"
		Private Const PROGRAM_XML_SETTING_PATH As String = "Settings/Path"

#End Region


#Region "Private fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		Private m_UpdateDatabaseAccess As NotifyDatabaseAccess

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private m_alarm As Threading.Timer

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml
		Private m_ProgSettingsXml As SettingsXml

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MasterMandantData As IEnumerable(Of MasterMandantData)
		Private m_AllowedMandantData As IEnumerable(Of MasterMandantData)


		Private m_MandantData As Mandant
		Private m_connectionString As String
		Private m_SonstigesSetting As String

		''' <summary>
		''' Service Uri of Sputnik notification util webservice.
		''' </summary>
		Private m_NotificationUtilWebServiceUri As String

		''' <summary>
		''' Service Uri of Sputnik scan job util webservice.
		''' </summary>
		Private m_ScanjobUtilWebServiceUri As String

		Private m_firstNotifyID As Integer?
		Private m_PopupMenu As DevExpress.XtraBars.PopupMenu

		Private gridDataList As New BindingList(Of NotifyData)
		Private m_ExitApplication As Boolean
		Private m_Timer As System.Timers.Timer
		Private m_TimerReport As System.Timers.Timer
		Private m_ChangeownReportForFinishingFlag As Boolean

		Private frmScanReport As frmReportDropIn
		Private frmScanCV As frmCVDropIn
		Private m_ReportUtility As ReportJobUtilities

		Private m_OriginalCustomerID As String
		Private m_IsProcessing As Boolean


		Private m_CurrentMDNumber As Integer
		Private m_CurrentMDString As String
		Private m_CurrentMDGuid As String

		Private m_CommonConfigFolder As String
		Private m_SputnikFileServer As String


		Private m_scanObj As ScanJobsUtilities
		Private m_ApplicantObj As ApplicantJobUtilities


#End Region


#Region "Contructor"

		Public Sub New(ByVal _setting As InitializeClass)

			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
			m_CurrentMDGuid = m_InitializationData.MDData.MDGuid

			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI

			m_SuppressUIEvents = True

			InitializeComponent()

			WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Text
			WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

			m_ExitApplication = False

			'm_NotificationUtilWebServiceUri = DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI
			'm_ScanjobUtilWebServiceUri = DEFAULT_SPUTNIK_SCANJOB_UTIL_WEBSERVICE_URI
			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
			m_ScanjobUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_SCANJOB_UTIL_WEBSERVICE_URI)


			Reset()
			TranslateControls()

			m_SuppressUIEvents = False

		End Sub

#End Region


#Region "private properties"

		''' <summary>
		''' Gets the selected notification.
		''' </summary>
		Private ReadOnly Property SelectedNotificationData As CustomerNotificationViewData
			Get
				Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim viewData = CType(grdView.GetRow(selectedRows(0)), CustomerNotificationViewData)
						Return viewData
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region


		Private Sub Reset()

			bbiSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
			If m_InitializationData.UserData.UserNr = 1 Then
				reNotifyHeader.ReadOnly = False
				reNotifyComments.ReadOnly = False

				bbiSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
			End If

			ResetNotificationGrid()
			ResetDetails()

		End Sub

		''' <summary>
		''' Resets Notification grid.
		''' </summary>
		Private Sub ResetNotificationGrid()

			gvTableContent.OptionsView.ShowIndicator = False
			gvTableContent.OptionsView.ShowAutoFilterRow = True
			gvTableContent.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvTableContent.OptionsView.ShowFooter = False
			gvTableContent.OptionsView.RowAutoHeight = True
			gvTableContent.Appearance.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			gvTableContent.PreviewFieldName = "NotifyComments"
			gvTableContent.PreviewLineCount = 3
			gvTableContent.PreviewIndent = 5
			gvTableContent.OptionsView.ShowPreview = True
			gvTableContent.OptionsBehavior.AllowAddRows = If(m_InitializationData.UserData.UserNr = 1, DevExpress.Utils.DefaultBoolean.True, DevExpress.Utils.DefaultBoolean.False)

			'Dim edit As RepositoryItemButtonEdit = New RepositoryItemButtonEdit
			'edit.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			'grdTableContent.RepositoryItems.Add(edit)

			Dim rich As RepositoryItemRichTextEdit = New RepositoryItemRichTextEdit
			rich.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.Default
			rich.DocumentFormat = DocumentFormat.Rtf
			grdTableContent.RepositoryItems.Add(rich)

			gvTableContent.Columns.Clear()


			Dim columnChecked As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChecked.OptionsColumn.AllowEdit = True
			columnChecked.Caption = m_Translate.GetSafeTranslationValue("Gesehen?")
			columnChecked.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnChecked.Name = "Checked"
			columnChecked.FieldName = "Checked"
			columnChecked.Visible = True
			columnChecked.Width = 10
			gvTableContent.Columns.Add(columnChecked)

			Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnID.OptionsColumn.AllowEdit = False
			columnID.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			columnID.Visible = False
			columnID.Width = 50
			gvTableContent.Columns.Add(columnID)

			Dim columnNotifyHeader As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNotifyHeader.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNotifyHeader.OptionsColumn.AllowEdit = False
			columnNotifyHeader.Caption = m_Translate.GetSafeTranslationValue("Betreff")
			columnNotifyHeader.Name = "NotifyHeader"
			columnNotifyHeader.FieldName = "NotifyHeader"
			columnNotifyHeader.Visible = True
			columnNotifyHeader.Width = 100
			columnNotifyHeader.ColumnEdit = rich
			gvTableContent.Columns.Add(columnNotifyHeader)

			Dim columnNotifyComments As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNotifyComments.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNotifyComments.OptionsColumn.AllowEdit = False
			columnNotifyComments.Caption = m_Translate.GetSafeTranslationValue("Beschreibung")
			columnNotifyComments.Name = "NotifyComments"
			columnNotifyComments.FieldName = "NotifyComments"
			columnNotifyComments.Visible = False
			columnNotifyComments.Width = 300
			columnNotifyComments.ColumnEdit = rich
			gvTableContent.Columns.Add(columnNotifyComments)

			Dim columnWhoCreated_FullData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnWhoCreated_FullData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnWhoCreated_FullData.OptionsColumn.AllowEdit = False
			columnWhoCreated_FullData.Caption = m_Translate.GetSafeTranslationValue("Erstellt")
			columnWhoCreated_FullData.Name = "WhoCreated_FullData"
			columnWhoCreated_FullData.FieldName = "WhoCreated_FullData"
			columnWhoCreated_FullData.Visible = True
			columnWhoCreated_FullData.Width = 60
			gvTableContent.Columns.Add(columnWhoCreated_FullData)

			Dim columnWhoChecked_FullData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnWhoChecked_FullData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnWhoChecked_FullData.OptionsColumn.AllowEdit = False
			columnWhoChecked_FullData.Caption = m_Translate.GetSafeTranslationValue("Gesehen")
			columnWhoChecked_FullData.Name = "WhoChecked_FullData"
			columnWhoChecked_FullData.FieldName = "WhoChecked_FullData"
			columnWhoChecked_FullData.Visible = False
			columnWhoChecked_FullData.Width = 60
			gvTableContent.Columns.Add(columnWhoChecked_FullData)


			grdTableContent.DataSource = Nothing

		End Sub

		Private Sub ResetDetails()

			reNotifyHeader.Text = String.Empty
			reNotifyComments.Text = Nothing

			bsiCreatedon.Caption = String.Empty
			bsiCreatedon.Caption = String.Empty

		End Sub

		''' <summary>
		'''  Translate controls
		''' </summary>
		Private Sub TranslateControls()

			Text = m_Translate.GetSafeTranslationValue(Text)
			chkExcludeChecked.Text = m_Translate.GetSafeTranslationValue(chkExcludeChecked.Text)

			lblBetreff.Text = m_Translate.GetSafeTranslationValue(lblBetreff.Text)
			lblBeschreibung.Text = m_Translate.GetSafeTranslationValue(lblBeschreibung.Text)

			bhiCreatedonLabel.Caption = m_Translate.GetSafeTranslationValue(bhiCreatedonLabel.Caption)
			bhiCheckedonLabel.Caption = m_Translate.GetSafeTranslationValue(bhiCheckedonLabel.Caption)

			bbiSave.Caption = m_Translate.GetSafeTranslationValue(bbiSave.Caption)

		End Sub

#Region "public methodes"

		Public Function LoadData() As Boolean
			Dim result As Boolean = True

			Try

				Dim success As Boolean = True
				SearchNotificationlistViaWebService()


			Catch ex As Exception

			End Try

		End Function

#End Region


		''' <summary>
		''' Search for notification over web service.
		''' </summary>
		Private Sub SearchNotificationlistViaWebService()

			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			m_firstNotifyID = Nothing

			grdTableContent.DataSource = Nothing

			Task(Of BindingList(Of CustomerNotificationViewData)).Factory.StartNew(Function() PerformNotificationlistWebserviceCallAsync(),
																								CancellationToken.None,
																								TaskCreationOptions.None,
																								TaskScheduler.Default).ContinueWith(Sub(t) FinishNotificationWebserviceCallTask(t), CancellationToken.None,
																																										TaskContinuationOptions.None, uiSynchronizationContext)

		End Sub

		''' <summary>
		'''  Performs notification check asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformNotificationlistWebserviceCallAsync() As BindingList(Of CustomerNotificationViewData)

			Dim listDataSource As BindingList(Of CustomerNotificationViewData) = New BindingList(Of CustomerNotificationViewData)

#If DEBUG Then
			'm_NotificationUtilWebServiceUri = "http://localhost:44721/SPNotification.asmx"
#End If

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.GetCustomerNotifications(m_CurrentMDGuid, If(chkExcludeChecked.Checked, m_InitializationData.UserData.UserGuid, String.Empty), chkExcludeChecked.Checked).ToList

			For Each result In searchResult

				Dim viewData = New CustomerNotificationViewData With {
						.ID = result.ID,
						.NotifyGroup = result.NotifyGroup,
						.CheckedFrom = result.CheckedFrom,
						.CheckedOn = result.CheckedOn,
						.CreatedFrom = result.CreatedFrom,
						.CreatedOn = result.CreatedOn,
						.NotifyComments = result.NotifyComments,
						.NotifyHeader = result.NotifyHeader,
						.Checked = (Not result.CheckedOn Is Nothing AndAlso result.User_ID = m_InitializationData.UserData.UserGuid)
					}

				If Not m_firstNotifyID.HasValue Then m_firstNotifyID = result.ID
				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function

		''' <summary>
		''' Finish notification web service call.
		''' </summary>
		Private Sub FinishNotificationWebserviceCallTask(ByVal t As Task(Of BindingList(Of CustomerNotificationViewData)))

			Try

				Select Case t.Status
					Case TaskStatus.RanToCompletion
						' Webservice call was successful.
						m_SuppressUIEvents = True
						grdTableContent.DataSource = t.Result

						FocusNotification(m_firstNotifyID)

						m_SuppressUIEvents = False

					Case TaskStatus.Faulted
						' Something went wrong -> log error.
						m_Logger.LogError(t.Exception.ToString())

					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try


		End Sub

		Private Sub OngvTableContent_RowUpdated(sender As Object, e As RowObjectEventArgs) Handles gvTableContent.RowUpdated
			Dim msg As String

			gvTableContent.PostEditor()
			gvTableContent.UpdateCurrentRow()

			grdTableContent.RefreshDataSource()

			grdTableContent.FocusedView.CloseEditor()
			Dim success = UpdateRecord(e.Row)

			If success Then
				msg = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
			Else
				msg = m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden.")
			End If

		End Sub

		Private Sub OngvTableContent_CustomDrawRowPreview(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs) Handles gvTableContent.CustomDrawRowPreview

			'Dim rich As RepositoryItemRichTextEdit = New RepositoryItemRichTextEdit
			'rich.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			'rich.DocumentFormat = DocumentFormat.Rtf
			''grdTableContent.RepositoryItems.Add(rich)

			'Dim vi As RichTextEditViewInfo = New RichTextEditViewInfo(rich) 'RepositoryItemRichTextEdit2)
			'Dim info As ProductInfo = CType(gvTableContent.GetRow(e.RowHandle), ProductInfo)
			'vi.LoadText(info.ToString)
			'vi.UpdatePaintAppearance
			'vi.PaintAppearance.FillRectangle(e.Cache, e.Bounds)
			'vi.CalcViewInfo(e.Graphics, System.Windows.Forms.MouseButtons.None, New Point(0, 0), e.Bounds)
			'DevExpress.XtraEditors.Drawing.RichTextEditPainter.DrawRTF(vi, e.Cache)

			'e.Handled = True

			Dim item = New RepositoryItemRichTextEdit
			Dim vi As RichTextEditViewInfo = New RichTextEditViewInfo(item)
			Dim grdview = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
			Dim info = TryCast(grdview.GetRow(e.RowHandle), CustomerNotificationViewData)
			'vi.LoadText(info.TextoHTML)
			vi.UpdatePaintAppearance()
			vi.CalcViewInfo(e.Graphics, System.Windows.Forms.MouseButtons.None, New Point(0, 0), e.Bounds)
			DevExpress.XtraEditors.Drawing.RichTextEditPainter.DrawRTF(vi, e.Cache)
			e.Handled = True
		End Sub


		Private Sub OnbbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
			Dim success As Boolean = True
			Dim SelectedData = SelectedNotificationData

			SelectedData.NotifyHeader = reNotifyHeader.RtfText
			SelectedData.NotifyComments = reNotifyComments.RtfText
			success = success AndAlso PerformUpdateAssignedNotificationWebservice(SelectedData, SelectedData.ID = 0)

		End Sub

		Private Function UpdateRecord(ByVal rowobject As Object) As Boolean
			Dim success As Boolean = True
			Dim data = grdTableContent.DataSource

			Dim SelectedData = CType(rowobject, CustomerNotificationViewData)
			If SelectedData.ID = 0 Then
			Else
			End If

			SelectedData.NotifyHeader = reNotifyHeader.RtfText
			SelectedData.NotifyComments = reNotifyComments.RtfText
			success = PerformUpdateAssignedNotificationWebservice(SelectedData, SelectedData.ID = 0)


			Return success

		End Function

		Private Function PerformUpdateAssignedNotificationWebservice(ByVal notifyData As CustomerNotificationViewData, ByVal saveRecordAsNew As Boolean) As Boolean

			Dim success As Boolean = True

#If DEBUG Then
			'm_NotificationUtilWebServiceUri = "http://localhost:44721/SPNotification.asmx"
#End If

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			' Read data over webservice
			If saveRecordAsNew Then
				notifyData.CreatedFrom = "System"
				success = success AndAlso webservice.AddCustomerNotification(m_CurrentMDGuid, notifyData.NotifyHeader, notifyData.NotifyComments, notifyData.CreatedFrom)

			Else
				If m_InitializationData.UserData.UserNr = 1 Then
					success = success AndAlso webservice.UpdateAssignedCustomerNotificationContent(m_CurrentMDGuid, notifyData.ID, notifyData.NotifyHeader, notifyData.NotifyComments, "System")

				Else
					success = success AndAlso webservice.UpdateAssignedCustomerNotification(m_CurrentMDGuid, notifyData.ID, notifyData.CheckedOn Is Nothing, notifyData.NotifyHeader, notifyData.NotifyComments,
																																									m_InitializationData.UserData.UserGuid, m_InitializationData.UserData.UserFullName)

				End If

			End If


			Return success

		End Function

		Private Sub chkExcludeChecked_CheckedChanged(sender As Object, e As EventArgs) Handles chkExcludeChecked.CheckedChanged
			SearchNotificationlistViaWebService()
		End Sub

		Sub OngvTableContent_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvTableContent.RowCellClick

			ResetDetails()
			Dim column = e.Column
			Dim dataRow = gvTableContent.GetRow(e.RowHandle)
			If dataRow Is Nothing Then
				Return
			Else
				LoadAssignedNotifyDetails()

			End If

		End Sub

		''' <summary>
		''' Focuses a notification.
		''' </summary>
		Private Sub FocusNotification(ByVal notifyID As Integer?)

			If Not notifyID.HasValue Then Return
			If Not grdTableContent.DataSource Is Nothing Then

				Dim notifyViewData = CType(gvTableContent.DataSource, BindingList(Of CustomerNotificationViewData))

				Dim index = notifyViewData.ToList().FindIndex(Function(data) data.ID = notifyID)

				m_SuppressUIEvents = True
				Dim rowHandle = gvTableContent.GetRowHandle(index)
				gvTableContent.FocusedRowHandle = rowHandle

				LoadAssignedNotifyDetails()

				m_SuppressUIEvents = False
			End If

		End Sub

		Private Sub LoadAssignedNotifyDetails()

			Dim data = SelectedNotificationData
			If data Is Nothing Then
				m_Logger.LogError("notification data could not be founded! " & m_InitializationData.MDData.MDDbConn)

				Return
			End If

			reNotifyHeader.RtfText = data.NotifyHeader
			reNotifyComments.RtfText = data.NotifyComments

			bsiCreatedon.Caption = data.WhoCreated_FullData
			bsiCheckedon.Caption = data.WhoChecked_FullData

		End Sub


#Region "helper class"

		''' <summary>
		''' notificaton search view data (tbl_Notify).
		''' </summary>
		Private Class CustomerNotificationViewData

			Public Property ID As Integer
			Public Property Customer_ID As String
			Public Property User_ID As String
			Public Property NotifyGroup As String
			Public Property NotifyHeader As String
			Public Property NotifyComments As String
			Public Property NotifyArt As NotifyEnum
			Public Property CreatedOn As DateTime?
			Public Property CreatedFrom As String
			Public Property CheckedOn As DateTime?
			Public Property CheckedFrom As String
			Public Property Checked() As Boolean

			Public Enum NotifyEnum
				NotDefined
				AsError
				AsImportant
				AsInfo
			End Enum

			Public ReadOnly Property WhoCreated_FullData As String
				Get
					Return String.Format("{0:G}", CreatedOn)
				End Get
			End Property

			Public ReadOnly Property WhoChecked_FullData As String
				Get
					If CheckedOn.HasValue Then
						Return String.Format("{0}, {1}", CheckedOn, CheckedFrom)
					Else
						Return String.Empty
					End If
				End Get
			End Property


		End Class



#End Region

	End Class

End Namespace
