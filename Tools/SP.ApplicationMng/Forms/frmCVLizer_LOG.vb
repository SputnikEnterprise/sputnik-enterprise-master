Imports System.ComponentModel
Imports System.IO
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports SP.ApplicationMng.CVLizer.DataObject

Namespace CVLizer

	Partial Class frmCVLizer

		''' <summary>
		''' Resets LOG grid.
		''' </summary>
		Private Sub ResetLOGGrid()

			gvLOG.OptionsView.ShowIndicator = False
			gvLOG.OptionsView.ShowAutoFilterRow = True
			gvLOG.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvLOG.OptionsView.ShowFooter = False
			gvLOG.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvLOG.Columns.Clear()

			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = ("Datum")
			columnCustomer_ID.Name = "LogDate"
			columnCustomer_ID.FieldName = "LogDate"
			columnCustomer_ID.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCustomer_ID.DisplayFormat.FormatString = "G"
			columnCustomer_ID.Width = 30
			columnCustomer_ID.Visible = True
			gvLOG.Columns.Add(columnCustomer_ID)

			Dim columnLogType As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLogType.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLogType.OptionsColumn.AllowEdit = False
			columnLogType.Caption = ("Art")
			columnLogType.Name = "LogType"
			columnLogType.FieldName = "LogType"
			columnLogType.Visible = True
			columnLogType.Width = 50
			gvLOG.Columns.Add(columnLogType)

			Dim columnMessage As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMessage.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMessage.OptionsColumn.AllowEdit = False
			columnMessage.Caption = ("Nachricht")
			columnMessage.Name = "Message"
			columnMessage.FieldName = "Message"
			columnMessage.Visible = True
			gvLOG.Columns.Add(columnMessage)


			grdLOG.DataSource = Nothing

		End Sub

		Private Sub ResetXMLFilesGrid()

			gvCVLXMLFiles.OptionsView.ShowIndicator = False
			gvCVLXMLFiles.OptionsView.ShowAutoFilterRow = True
			gvCVLXMLFiles.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvCVLXMLFiles.OptionsView.ShowFooter = False
			gvCVLXMLFiles.OptionsView.AllowCellMerge = True
			gvCVLXMLFiles.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvCVLXMLFiles.Columns.Clear()

			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = ("Customer_ID")
			columnCustomer_ID.Name = "Customer_ID"
			columnCustomer_ID.FieldName = "Customer_ID"
			columnCustomer_ID.Width = 30
			columnCustomer_ID.Visible = False
			gvCVLXMLFiles.Columns.Add(columnCustomer_ID)

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerName.OptionsColumn.AllowEdit = False
			columnCustomerName.Caption = ("CustomerName")
			columnCustomerName.Name = "CustomerName"
			columnCustomerName.FieldName = "CustomerName"
			columnCustomerName.Width = 100
			columnCustomerName.Visible = True
			gvCVLXMLFiles.Columns.Add(columnCustomerName)

			Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLONr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLONr.OptionsColumn.AllowEdit = False
			columnLONr.Caption = ("Month-Year")
			columnLONr.Name = "MonthYear"
			columnLONr.FieldName = "MonthYear"
			columnLONr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnLONr.AppearanceHeader.Options.UseTextOptions = True
			columnLONr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnLONr.AppearanceCell.Options.UseTextOptions = True
			columnLONr.Width = 50
			columnLONr.Visible = True
			gvCVLXMLFiles.Columns.Add(columnLONr)

			Dim columnDateien As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDateien.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDateien.OptionsColumn.AllowEdit = False
			columnDateien.Caption = ("MonthlyUsed")
			columnDateien.Name = "UsedAmount"
			columnDateien.FieldName = "UsedAmount"
			columnDateien.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnDateien.AppearanceHeader.Options.UseTextOptions = True
			columnDateien.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnDateien.AppearanceCell.Options.UseTextOptions = True
			columnDateien.Visible = True
			columnDateien.Width = 20
			gvCVLXMLFiles.Columns.Add(columnDateien)

			Dim columnTotalUsedAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTotalUsedAmount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnTotalUsedAmount.OptionsColumn.AllowEdit = False
			columnTotalUsedAmount.Caption = ("YearlyUsed")
			columnTotalUsedAmount.Name = "TotalUsedAmount"
			columnTotalUsedAmount.FieldName = "TotalUsedAmount"
			columnTotalUsedAmount.Visible = True
			columnTotalUsedAmount.Width = 50
			gvCVLXMLFiles.Columns.Add(columnTotalUsedAmount)


			grdCVLXMLFiles.DataSource = Nothing

		End Sub

		Private Sub AddLogEntry()

			Try
				Dim existsScanData As List(Of EntryLOGData) = CType(grdLOG.DataSource, List(Of EntryLOGData))
				Dim newData As List(Of EntryLOGData) = m_LogData

				grdLOG.DataSource = Nothing

				If existsScanData Is Nothing OrElse existsScanData.Count = 0 Then
					existsScanData = New List(Of EntryLOGData)
					existsScanData = newData

				Else
					existsScanData.Concat(m_LogData)
					'existsScanData.Add(m_LogData)

				End If
				grdLOG.DataSource = existsScanData

			Catch ex As Exception

			End Try

		End Sub

		Private Sub LoadXMLFileData()
			Dim year As Integer = Convert.ToDateTime(deDate.EditValue).Year
			Dim customerTotalUsed = 0
			Dim xmlPath As String = Path.Combine(m_SettingFile.CVLXMLFolder)
			Dim cvlUsage As New List(Of CVLUsageData)

			Dim customerData = LoadCVLCustomerData()
			Dim customerName As String = String.Empty

			Dim customerList As List(Of String) = Directory.GetDirectories(xmlPath, "*", System.IO.SearchOption.AllDirectories).ToList

			For Each customer In customerList
				If String.IsNullOrWhiteSpace(customer.ToString) Then Continue For
				Dim directoryToSearch As New DirectoryInfo(customer)
				Dim data As New List(Of CVLUsageData)
				Dim assignedCustomer = customerData.Where(Function(x) x.Customer_ID = directoryToSearch.Name).FirstOrDefault
				If Not assignedCustomer Is Nothing Then
					customerName = assignedCustomer.CustomerName
				Else
					customerName = String.Format("{0} - Not defined!!!", directoryToSearch.Name)
				End If

				Dim filesCountByCustomer = directoryToSearch.EnumerateFiles("*xml").Where(Function(f) f.CreationTime.Date.Year = year).OrderBy(Function(f) f.CreationTime).Select(Function(f) f.Name).ToList
				customerTotalUsed = filesCountByCustomer.Count

				Dim monthlyData = New List(Of MonthlyAmount)
				For i = 1 To Convert.ToDateTime(deDate.EditValue).Month
					Dim filesByDate = directoryToSearch.EnumerateFiles("*xml").Where(Function(f) f.CreationTime.Date.Month = i And f.CreationTime.Date.Year = year).OrderBy(Function(f) f.CreationTime).Select(Function(f) f.Name).ToList

					If Not filesByDate Is Nothing AndAlso filesByDate.Count > 0 Then
						cvlUsage.Add(New CVLUsageData With {.Customer_ID = directoryToSearch.Name, .CustomerName = customerName, .UsedAmount = filesByDate.Count, .UsedMonth = i, .UsedYear = year, .TotalUsedAmount = customerTotalUsed})
					End If

				Next

			Next

			grdCVLXMLFiles.DataSource = cvlUsage

		End Sub

		Private Function LoadCVLCustomerData() As IEnumerable(Of CVLCustomerViewData)

			Try
				SplashScreenManager.CloseForm(False)
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
				SplashScreenManager.Default.SetWaitFormCaption(String.Format(("Ihre Daten werden abgerufen.")) & Space(100))
				SplashScreenManager.Default.SetWaitFormDescription(("Dies kann einige Sekunden dauern") & "...")


				Dim webservice As New SP.Internal.Automations.SPApplicationWebService.SPApplicationSoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

				' Read data over webservice
				Dim profileDate As DateTime? = Nothing
				profileDate = deDate.EditValue

				If profileDate Is Nothing Then profileDate = Now.Date
				Dim searchResult = webservice.LoadCVLCustomerData(String.Empty)

				If searchResult Is Nothing Then
					m_Logger.LogError(String.Format("customer data for cvl xml files could not be loaded from webservice!"))

					Return Nothing
				End If

				Dim gridData = (From person In searchResult
								Select New CVLCustomerViewData With {.Customer_ID = person.Customer_ID,
									.CustomerName = person.CustomerName,
									.Location = person.Location,
									.CustomerNumber = person.CustomerNumber,
									.CustomerGroupNumber = person.CustomerGroupNumber
									}).ToList()

				Dim listDataSource As BindingList(Of CVLCustomerViewData) = New BindingList(Of CVLCustomerViewData)
				For Each p In gridData
					listDataSource.Add(p)
				Next


				Return listDataSource

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing
			Finally
				SplashScreenManager.CloseForm(False)

			End Try

		End Function


		Private Class CVLCustomerViewData
			Inherits SP.Internal.Automations.SPApplicationWebService.CVLizerCustomerDataDTO

		End Class


	End Class

End Namespace
