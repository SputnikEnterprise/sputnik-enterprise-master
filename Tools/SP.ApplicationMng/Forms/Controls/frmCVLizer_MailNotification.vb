
Imports System.ComponentModel
Imports System.IO
Imports System.IO.Directory
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
'Imports System.IO.File
Imports DevExpress.XtraSplashScreen

Namespace CVLizer

	Partial Class frmCVLizer

#Region "private property"

		'''' <summary>
		'''' Gets the selected doc scan as attachment.
		'''' </summary>
		'Private ReadOnly Property SelectedDocumentViewData As DocScanLocalViewData
		'	Get
		'		Dim grdView = TryCast(grdMailnotification.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

		'		If Not (grdView Is Nothing) Then

		'			Dim selectedRows = grdView.GetSelectedRows()

		'			If (selectedRows.Count > 0) Then
		'				Dim viewData = CType(grdView.GetRow(selectedRows(0)), DocScanLocalViewData)
		'				Return viewData
		'			End If

		'		End If

		'		Return Nothing
		'	End Get

		'End Property


#End Region

		Private Sub ResetMailNotificationGrid()

			gvMailNotification.OptionsView.ShowIndicator = False
			gvMailNotification.OptionsView.ShowAutoFilterRow = False
			gvMailNotification.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvMailNotification.OptionsView.ShowFooter = False
			gvMailNotification.OptionsView.AllowCellMerge = True
			gvMailNotification.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvMailNotification.Columns.Clear()


			Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnID.OptionsColumn.AllowEdit = False
			columnID.Caption = ("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			columnID.Visible = False
			columnID.Width = 10
			gvMailNotification.Columns.Add(columnID)

			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = ("Customer_ID")
			columnCustomer_ID.Name = "Customer_ID"
			columnCustomer_ID.FieldName = "Customer_ID"
			columnCustomer_ID.Visible = False
			columnCustomer_ID.Width = 60
			gvMailNotification.Columns.Add(columnCustomer_ID)

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerName.OptionsColumn.AllowEdit = False
			columnCustomerName.Caption = ("Name")
			columnCustomerName.Name = "CustomerName"
			columnCustomerName.FieldName = "CustomerName"
			columnCustomerName.Visible = True
			columnCustomerName.Width = 40
			columnCustomerName.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			gvMailNotification.Columns.Add(columnCustomerName)

			Dim columnCustomerLocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerLocation.OptionsColumn.AllowEdit = False
			columnCustomerLocation.Caption = ("Location")
			columnCustomerLocation.Name = "CustomerLocation"
			columnCustomerLocation.FieldName = "CustomerLocation"
			columnCustomerLocation.Visible = False
			columnCustomerLocation.Width = 40
			gvMailNotification.Columns.Add(columnCustomerLocation)

			Dim columnMailFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMailFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMailFrom.OptionsColumn.AllowEdit = False
			columnMailFrom.Caption = ("MailFrom")
			columnMailFrom.Name = "MailFrom"
			columnMailFrom.FieldName = "MailFrom"
			columnMailFrom.Width = 10
			columnMailFrom.Visible = False
			gvMailNotification.Columns.Add(columnMailFrom)

			Dim columnMailTo As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMailTo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMailTo.OptionsColumn.AllowEdit = False
			columnMailTo.Caption = ("MailTo")
			columnMailTo.Name = "MailTo"
			columnMailTo.FieldName = "MailTo"
			columnMailTo.Width = 40
			columnMailTo.Visible = True
			gvMailNotification.Columns.Add(columnMailTo)

			Dim columnMailSubject As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMailSubject.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMailSubject.OptionsColumn.AllowEdit = False
			columnMailSubject.Caption = ("Subject")
			columnMailSubject.Name = "MailSubject"
			columnMailSubject.FieldName = "MailSubject"
			columnMailSubject.Width = 10
			columnMailSubject.Visible = False
			gvMailNotification.Columns.Add(columnMailSubject)

			Dim columnMailBody As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMailBody.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMailBody.OptionsColumn.AllowEdit = False
			columnMailBody.Caption = ("Body")
			columnMailBody.Name = "MailBody"
			columnMailBody.FieldName = "MailBody"
			columnMailBody.Width = 10
			columnMailBody.Visible = False
			gvMailNotification.Columns.Add(columnMailBody)

			Dim columnDocLink As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDocLink.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDocLink.OptionsColumn.AllowEdit = False
			columnDocLink.Caption = ("DocLink")
			columnDocLink.Name = "DocLink"
			columnDocLink.FieldName = "DocLink"
			columnDocLink.Width = 40
			columnDocLink.Visible = True
			gvMailNotification.Columns.Add(columnDocLink)

			Dim columnRecipientGuid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRecipientGuid.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnRecipientGuid.OptionsColumn.AllowEdit = False
			columnRecipientGuid.Caption = ("RecipientGuid")
			columnRecipientGuid.Name = "RecipientGuid"
			columnRecipientGuid.FieldName = "RecipientGuid"
			columnRecipientGuid.Width = 40
			columnRecipientGuid.Visible = False
			gvMailNotification.Columns.Add(columnRecipientGuid)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.OptionsColumn.AllowEdit = False
			columnCreatedOn.Caption = ("CreatedOn")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.DisplayFormat.FormatString = "G"
			columnCreatedOn.Width = 40
			columnCreatedOn.Visible = True
			gvMailNotification.Columns.Add(columnCreatedOn)

			Dim item As New GridGroupSummaryItem() With {.FieldName = "CustomerName", .SummaryType = DevExpress.Data.SummaryItemType.Count}
			gvMailNotification.GroupSummary.Add(item)


			grdMailnotification.DataSource = Nothing

		End Sub

		Private Sub ResetAdvisorLoginGrid()

			gvAdvisorLogin.OptionsView.ShowIndicator = False
			gvAdvisorLogin.OptionsView.ShowAutoFilterRow = False
			gvAdvisorLogin.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvAdvisorLogin.OptionsView.ShowFooter = False
			gvAdvisorLogin.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			gvAdvisorLogin.OptionsView.AllowHtmlDrawGroups = True
			gvAdvisorLogin.OptionsView.AllowCellMerge = True
			gvAdvisorLogin.OptionsView.AllowCellMerge = True

			gvAdvisorLogin.Columns.Clear()

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerName.OptionsColumn.AllowEdit = False
			columnCustomerName.Caption = ("Name")
			columnCustomerName.Name = "CustomerName"
			columnCustomerName.FieldName = "CustomerName"
			columnCustomerName.Visible = True
			columnCustomerName.Width = 40
			'columnCustomerName.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			gvAdvisorLogin.Columns.Add(columnCustomerName)

			Dim columnAdvisorname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisorname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdvisorname.OptionsColumn.AllowEdit = False
			columnAdvisorname.Caption = ("Advisorname")
			columnAdvisorname.Name = "Advisorname"
			columnAdvisorname.FieldName = "Advisorname"
			columnAdvisorname.Width = 50
			columnAdvisorname.Visible = True
			columnAdvisorname.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			gvAdvisorLogin.Columns.Add(columnAdvisorname)


			Dim item As New GridGroupSummaryItem() With {.FieldName = "Advisorname", .SummaryType = DevExpress.Data.SummaryItemType.Count}
			gvAdvisorLogin.GroupSummary.Add(item)

			grdAdvisorLogin.DataSource = Nothing

		End Sub

		Private Sub ResetAdvisorMontlyLoginGrid()

			gvMontlyLogins.OptionsView.ShowIndicator = False
			gvMontlyLogins.OptionsView.ShowAutoFilterRow = False
			gvMontlyLogins.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvMontlyLogins.OptionsView.ShowFooter = False
			gvMontlyLogins.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			gvMontlyLogins.OptionsView.AllowHtmlDrawGroups = True
			gvMontlyLogins.OptionsView.AllowCellMerge = True

			gvMontlyLogins.Columns.Clear()

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerName.OptionsColumn.AllowEdit = False
			columnCustomerName.Caption = ("Name")
			columnCustomerName.Name = "CustomerName"
			columnCustomerName.FieldName = "CustomerName"
			columnCustomerName.Visible = True
			columnCustomerName.Width = 40
			'columnCustomerName.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			gvMontlyLogins.Columns.Add(columnCustomerName)

			Dim columnAdvisorname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisorname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdvisorname.OptionsColumn.AllowEdit = False
			columnAdvisorname.Caption = ("Advisorname")
			columnAdvisorname.Name = "Advisorname"
			columnAdvisorname.FieldName = "Advisorname"
			columnAdvisorname.Width = 50
			columnAdvisorname.Visible = True
			columnAdvisorname.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			gvMontlyLogins.Columns.Add(columnAdvisorname)


			Dim item As New GridGroupSummaryItem() With {.FieldName = "Advisorname", .SummaryType = DevExpress.Data.SummaryItemType.Count}
			gvMontlyLogins.GroupSummary.Add(item)

			grdMonthlyLogins.DataSource = Nothing

		End Sub

		'Sub OngvMailNotification_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvMailNotification.RowCellClick
		'	Dim success As Boolean = True

		'	Dim data = SelectedDocumentViewData
		'	If data Is Nothing Then
		'		SplashScreenManager.CloseForm(False)
		'		m_UtilityUI.ShowErrorDialog(("Scan Daten konnte nicht geladen werden."))
		'		Return
		'	End If

		'	If e.Clicks = 2 Then
		'		Dim docData = PerformAssignedDocScanWebservice(data.Customer_ID, data.ID)
		'		If docData Is Nothing OrElse docData.ScanContent Is Nothing Then
		'			SplashScreenManager.CloseForm(False)
		'			m_UtilityUI.ShowErrorDialog(("Selektiertes Scan konnte nicht geladen werden."))
		'			Return
		'		End If

		'		success = success AndAlso DisplayAssignedMailNotificationData(docData)
		'	End If


		'End Sub

		Private Function LoadMailNotificationData() As Boolean

			Try
				SplashScreenManager.CloseForm(False)
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
				SplashScreenManager.Default.SetWaitFormCaption(String.Format(("Ihre Daten werden abgerufen.")) & Space(100))
				SplashScreenManager.Default.SetWaitFormDescription(("Dies kann einige Sekunden dauern") & "...")


				Dim webservice As New SP.Internal.Automations.SPNotificationWebService.SPNotificationSoapClient ' .SPScanJobUtilitySoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

				' Read data over webservice
				Dim profileDate As DateTime? = Nothing
				profileDate = deDate.EditValue

				If profileDate Is Nothing Then profileDate = Now.Date
				Dim searchResult = webservice.LoadMailNotificationbyDateData(String.Empty, profileDate)

				If searchResult Is Nothing Then
					m_Logger.LogError(String.Format("mail notifications could not be loaded from webservice! {0} | {1}", m_customerID, profileDate))

					Return False
				End If

				Dim gridData = (From person In searchResult
								Select New MailNotificationViewData With {.ID = person.ID,
															.Customer_ID = person.Customer_ID,
															.CustomerName = person.CustomerName,
															.CustomerLocation = person.CustomerLocation,
															.MailFrom = person.MailFrom,
															.MailTo = person.MailTo,
															.Result = person.Result,
															.MailSubject = person.MailSubject,
															.MailBody = person.MailBody,
															.DocLink = person.DocLink,
															.RecipientGuid = person.RecipientGuid,
															.CreatedOn = person.CreatedOn
															}).ToList()

				Dim listDataSource As BindingList(Of MailNotificationViewData) = New BindingList(Of MailNotificationViewData)
				For Each p In gridData
					listDataSource.Add(p)
				Next

				grdMailnotification.DataSource = listDataSource
				bsiMainRecordCount.Caption = String.Format("Anzahl Datensätze (Versandte Nachrichten): {0} | Anzahl Datensätze (Angemeldete Benutzer): {1}", gvMailNotification.RowCount, gvAdvisorLogin.RowCount)


				Return Not listDataSource Is Nothing

			Catch ex As Exception

				Return Nothing
			Finally
				SplashScreenManager.CloseForm(False)

			End Try

		End Function

		Private Function LoadAdvisorData() As Boolean
			Dim datalist As New List(Of NotScanedDocfiles)

			Try
				SplashScreenManager.CloseForm(False)
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
				SplashScreenManager.Default.SetWaitFormCaption(String.Format(("Ihre Daten werden abgerufen.")) & Space(100))
				SplashScreenManager.Default.SetWaitFormDescription(("Dies kann einige Sekunden dauern") & "...")

				Dim webservice As New SP.Internal.Automations.SPNotificationWebService.SPNotificationSoapClient ' SPScanJobUtilitySoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

				' Read data over webservice
				Dim profileDate As DateTime? = Nothing
				profileDate = deDate.EditValue

				If profileDate Is Nothing Then profileDate = Now.Date
				Dim searchResult = webservice.LoadAdvisorLoginByDateData(String.Empty, profileDate)

				If searchResult Is Nothing Then
					m_Logger.LogError(String.Format("advisor login data could not be loaded from webservice! {0} | {1}", m_customerID, profileDate))

					Return False
				End If

				Dim gridData = (From person In searchResult
								Select New AdvisorLoginViewData With {.UserCount = person.UserCount,
															.Customer_ID = person.Customer_ID,
															.CustomerName = person.CustomerName,
															.Advisorname = person.Advisorname,
															.LogYear = person.LogYear,
															.LogMonth = person.LogMonth,
															.CreatedOn = person.CreatedOn
															}).ToList()

				Dim listDataSource As BindingList(Of AdvisorLoginViewData) = New BindingList(Of AdvisorLoginViewData)
				For Each p In gridData
					listDataSource.Add(p)
				Next

				grdAdvisorLogin.DataSource = listDataSource
				bsiMainRecordCount.Caption = String.Format("Anzahl Datensätze (Versandte Nachrichten): {0} | Anzahl Datensätze (Angemeldete Benutzer): {1}", gvMailNotification.RowCount, gvAdvisorLogin.RowCount)


				Return Not listDataSource Is Nothing

			Catch ex As Exception

				Return Nothing
			Finally
				SplashScreenManager.CloseForm(False)

			End Try

			Return Not (datalist Is Nothing)
		End Function

		Private Function LoadAdvisorMonthlyData() As Boolean
			Dim datalist As New List(Of NotScanedDocfiles)

			Try
				SplashScreenManager.CloseForm(False)
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
				SplashScreenManager.Default.SetWaitFormCaption(String.Format(("Ihre Daten werden abgerufen.")) & Space(100))
				SplashScreenManager.Default.SetWaitFormDescription(("Dies kann einige Sekunden dauern") & "...")

				Dim webservice As New SP.Internal.Automations.SPNotificationWebService.SPNotificationSoapClient ' SPScanJobUtilitySoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

				' Read data over webservice
				Dim profileDate As DateTime? = Nothing
				profileDate = deDate.EditValue

				If profileDate Is Nothing Then profileDate = Now.Date
				Dim searchResult = webservice.LoadAdvisorMonthlyLoginByDateData(String.Empty, profileDate)

				If searchResult Is Nothing Then
					m_Logger.LogError(String.Format("advisor login data could not be loaded from webservice! {0} | {1}", m_customerID, profileDate))

					Return False
				End If

				Dim gridData = (From person In searchResult
								Select New AdvisorLoginViewData With {.UserCount = person.UserCount,
															.CustomerName = person.CustomerName,
															.Advisorname = person.Advisorname
															}).ToList()

				Dim listDataSource As BindingList(Of AdvisorLoginViewData) = New BindingList(Of AdvisorLoginViewData)
				For Each p In gridData
					listDataSource.Add(p)
				Next

				grdMonthlyLogins.DataSource = listDataSource


				Return Not listDataSource Is Nothing

			Catch ex As Exception

				Return Nothing
			Finally
				SplashScreenManager.CloseForm(False)

			End Try

			Return Not (datalist Is Nothing)
		End Function

#Region "Helper class"

		Private Class MailNotificationViewData
			Inherits SP.Internal.Automations.SPNotificationWebService.EMailNotificationDTO

		End Class

		Private Class AdvisorLoginViewData
			Inherits SP.Internal.Automations.SPNotificationWebService.AdvisorLoginData

			Public ReadOnly Property Month_Year As String
				Get
					Return String.Format("{0} - {1}", LogMonth, LogYear)
				End Get
			End Property

		End Class


#End Region


	End Class

End Namespace
