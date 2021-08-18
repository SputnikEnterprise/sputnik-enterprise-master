
Imports System.ComponentModel
Imports System.IO
Imports System.IO.Directory
Imports DevExpress.XtraGrid
'Imports System.IO.File
Imports DevExpress.XtraSplashScreen

Namespace CVLizer

	Partial Class frmCVLizer

#Region "private property"

		''' <summary>
		''' Gets the selected doc scan as attachment.
		''' </summary>
		Private ReadOnly Property SelectedDocumentViewData As DocScanLocalViewData
			Get
				Dim grdView = TryCast(grdDocScan.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim viewData = CType(grdView.GetRow(selectedRows(0)), DocScanLocalViewData)
						Return viewData
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region

		Private Sub ResetDocScanGrid()

			gvDocScan.OptionsView.ShowIndicator = False
			gvDocScan.OptionsView.ShowAutoFilterRow = True
			gvDocScan.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvDocScan.OptionsView.ShowFooter = False
			gvDocScan.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			'gvDocScan.OptionsView.AllowCellMerge = True

			gvDocScan.Columns.Clear()


			Dim columnProfileID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnProfileID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnProfileID.OptionsColumn.AllowEdit = False
			columnProfileID.Caption = ("ID")
			columnProfileID.Name = "ID"
			columnProfileID.FieldName = "ID"
			columnProfileID.Visible = False
			columnProfileID.Width = 10
			gvDocScan.Columns.Add(columnProfileID)

			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = ("Customer_ID")
			columnCustomer_ID.Name = "Customer_ID"
			columnCustomer_ID.FieldName = "Customer_ID"
			columnCustomer_ID.Visible = True
			columnCustomer_ID.Width = 60
			gvDocScan.Columns.Add(columnCustomer_ID)

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerName.OptionsColumn.AllowEdit = False
			columnCustomerName.Caption = ("Name")
			columnCustomerName.Name = "CustomerName"
			columnCustomerName.FieldName = "CustomerName"
			columnCustomerName.Visible = True
			columnCustomerName.Width = 40
			columnCustomerName.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			gvDocScan.Columns.Add(columnCustomerName)

			Dim columnWorkID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnWorkID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnWorkID.OptionsColumn.AllowEdit = False
			columnWorkID.Caption = ("ImportedFileGuid")
			columnWorkID.Name = "ImportedFileGuid"
			columnWorkID.FieldName = "ImportedFileGuid"
			columnWorkID.Width = 10
			columnWorkID.Visible = False
			gvDocScan.Columns.Add(columnWorkID)

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
			gvDocScan.Columns.Add(columnCreatedOn)

			Dim columnCheckedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCheckedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCheckedOn.OptionsColumn.AllowEdit = False
			columnCheckedOn.Caption = ("CheckedOn")
			columnCheckedOn.Name = "CheckedOn"
			columnCheckedOn.FieldName = "CheckedOn"
			columnCheckedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCheckedOn.DisplayFormat.FormatString = "G"
			columnCheckedOn.Width = 40
			columnCheckedOn.Visible = True
			gvDocScan.Columns.Add(columnCheckedOn)

			Dim item As New GridGroupSummaryItem() With {.FieldName = "CustomerName", .SummaryType = DevExpress.Data.SummaryItemType.Count}
			gvDocScan.GroupSummary.Add(item)

			grdDocScan.DataSource = Nothing

		End Sub

		Private Sub ResetNotScanedFilesGrid()

			gvNotScanedFiles.OptionsView.ShowIndicator = False
			gvNotScanedFiles.OptionsView.ShowAutoFilterRow = True
			gvNotScanedFiles.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvNotScanedFiles.OptionsView.ShowFooter = False
			gvNotScanedFiles.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvNotScanedFiles.Columns.Clear()

			Dim columnCustomerID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerID.OptionsColumn.AllowEdit = False
			columnCustomerID.Caption = ("Customer_ID")
			columnCustomerID.Name = "CustomerID"
			columnCustomerID.FieldName = "CustomerID"
			columnCustomerID.Visible = True
			columnCustomerID.Width = 60
			gvNotScanedFiles.Columns.Add(columnCustomerID)

			Dim columnFilename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFilename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFilename.OptionsColumn.AllowEdit = False
			columnFilename.Caption = ("Filename")
			columnFilename.Name = "Filename"
			columnFilename.FieldName = "Filename"
			columnFilename.Width = 10
			columnFilename.Visible = True
			gvNotScanedFiles.Columns.Add(columnFilename)

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
			gvNotScanedFiles.Columns.Add(columnCreatedOn)


			grdNotScanedFiles.DataSource = Nothing

		End Sub

		Sub OngvDocScan_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvDocScan.RowCellClick
			Dim success As Boolean = True

			Dim data = SelectedDocumentViewData
			If data Is Nothing Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(("Scan Daten konnte nicht geladen werden."))
				Return
			End If

			If e.Clicks = 2 Then
				Dim docData = PerformAssignedDocScanWebservice(data.Customer_ID, data.ID)
				If docData Is Nothing OrElse docData.ScanContent Is Nothing Then
					SplashScreenManager.CloseForm(False)
					m_UtilityUI.ShowErrorDialog(("Selektiertes Scan konnte nicht geladen werden."))
					Return
				End If

				success = success AndAlso DisplayAssignedDocScanData(docData)
			End If


		End Sub

		Private Function LoadDocScanData() As Boolean

			Try
				SplashScreenManager.CloseForm(False)
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
				SplashScreenManager.Default.SetWaitFormCaption(String.Format(("Ihre Daten werden abgerufen.")) & Space(100))
				SplashScreenManager.Default.SetWaitFormDescription(("Dies kann einige Sekunden dauern") & "...")


				Dim webservice As New SP.Main.Notify.SPScanJobWebService.SPScanJobUtilitySoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ScanUtilWebServiceUri)

				' Read data over webservice
				Dim profileDate As DateTime? = Nothing
				profileDate = deDate.EditValue

				If profileDate Is Nothing Then profileDate = Now.Date
				Dim searchResult = webservice.LoadAllScanJobData(String.Empty, 0, profileDate)

				If searchResult Is Nothing Then
					m_Logger.LogError(String.Format("Documents could not be loaded from webservice! {0} | {1}", m_customerID, 0))

					Return False
				End If

				Dim gridData = (From person In searchResult
								Select New DocScanLocalViewData With {.ID = person.ID,
															.Customer_ID = person.Customer_ID,
															.CustomerName = person.CustomerName,
															.BusinessBranchNumber = person.BusinessBranchNumber,
															.ModulNumber = person.ModulNumber,
															.DocumentCategoryNumber = person.DocumentCategoryNumber,
															.CreatedOn = person.CreatedOn,
															.CreatedFrom = person.CreatedFrom,
															.CheckedOn = person.CheckedOn,
															.CheckedFrom = person.CheckedFrom,
															.ImportedFileGuid = person.ImportedFileGuid,
															.ScanContent = person.ScanContent
															}).ToList()

				Dim listDataSource As BindingList(Of DocScanLocalViewData) = New BindingList(Of DocScanLocalViewData)
				For Each p In gridData
					listDataSource.Add(p)
				Next

				grdDocScan.DataSource = listDataSource
				bsiMainRecordCount.Caption = String.Format("Anzahl Datensätze (Gescannte Dokumente): {0} | Anzahl Datensätze (NICHT Gescannte Dokumente): {1}", gvDocScan.RowCount, gvNotScanedFiles.RowCount)


				Return Not listDataSource Is Nothing

			Catch ex As Exception

				Return Nothing
			Finally
				SplashScreenManager.CloseForm(False)

			End Try

		End Function

		Private Function PerformAssignedDocScanWebservice(ByVal customer_ID As String, ByVal recID As Integer?) As DocScanLocalViewData

			Dim webservice As New SP.Main.Notify.SPScanJobWebService.SPScanJobUtilitySoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ScanUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.LoadAssignedScanJobData(customer_ID, recID)

			Dim gridData = New DocScanLocalViewData With {.ID = searchResult.ID,
															.Customer_ID = searchResult.Customer_ID,
															.CustomerName = searchResult.CustomerName,
															.BusinessBranchNumber = searchResult.BusinessBranchNumber,
															.ModulNumber = searchResult.ModulNumber,
															.DocumentCategoryNumber = searchResult.DocumentCategoryNumber,
															.CreatedOn = searchResult.CreatedOn,
															.CreatedFrom = searchResult.CreatedFrom,
															.CheckedOn = searchResult.CheckedOn,
															.CheckedFrom = searchResult.CheckedFrom,
															.ImportedFileGuid = searchResult.ImportedFileGuid,
															.ScanContent = searchResult.ScanContent
															}

			Return gridData
		End Function

		Private Function DisplayAssignedDocScanData(ByVal data As DocScanLocalViewData) As Boolean
			Dim success As Boolean = True

			Dim tmpFilename = Path.GetTempFileName()
			tmpFilename = Path.ChangeExtension(tmpFilename, "pdf")
			success = success AndAlso m_Utility.WriteFileBytes(tmpFilename, data.ScanContent)

			If success Then
				m_Utility.OpenFileWithDefaultProgram(tmpFilename)
			End If

			Return success

		End Function

		Private Function LoadNotScanedFiles() As Boolean
			Dim datalist As New List(Of NotScanedDocfiles)

			Try
				SplashScreenManager.CloseForm(False)
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
				SplashScreenManager.Default.SetWaitFormCaption(String.Format(("Ihre Daten werden abgerufen.")) & Space(100))
				SplashScreenManager.Default.SetWaitFormDescription(("Dies kann einige Sekunden dauern") & "...")

				Dim notScanfiles = LoadNotrecognaizedScans(m_SettingFile.ScanDirectoryToListen, {m_SettingFile.ScanFileFilter})

				grdNotScanedFiles.DataSource = Nothing
				If notScanfiles Is Nothing OrElse notScanfiles.Count = 0 Then
					SplashScreenManager.CloseForm(False)

					Return False
				End If

				For Each itm In notScanfiles
					Dim data As New NotScanedDocfiles

					data.CustomerID = itm.Directory.Name
					data.Filename = itm.Name
					data.CreatedOn = itm.CreationTime

					datalist.Add(data)

				Next

				grdNotScanedFiles.DataSource = datalist
				bsiMainRecordCount.Caption = String.Format("Anzahl Datensätze (Gescannte Dokumente): {0} | Anzahl Datensätze (NICHT Gescannte Dokumente): {1}", gvDocScan.RowCount, gvNotScanedFiles.RowCount)


			Catch ex As Exception
				SplashScreenManager.CloseForm(False)

			Finally
				SplashScreenManager.CloseForm(False)

			End Try

			Return Not (datalist Is Nothing)

		End Function

		Private Function LoadNotrecognaizedScans(ByVal RootFolder As String, ByVal FileFilter() As String) As List(Of FileInfo)
			Dim ReturnedData As New List(Of FileInfo)                             'List to hold the search results
			Dim FolderStack As New Stack(Of String)                             'Stack for searching the folders

			FolderStack.Push(RootFolder)                                        'Start at the specified root folder
			Do While FolderStack.Count > 0                                      'While there are things in the stack
				Dim ThisFolder As String = FolderStack.Pop                      'Grab the next folder to process
				Try                                                             'Use a try to catch any errors
					For Each SubFolder In GetDirectories(ThisFolder)            'Loop through each sub folder in this folder
						FolderStack.Push(SubFolder)                             'Add to the stack for further processing
					Next                                                        'Process next sub folder

					For Each FileExt In FileFilter                              'For each File filter specified
						Dim foundedFiles() As FileInfo = New DirectoryInfo(ThisFolder).GetFiles(FileExt)

						For Each itm In foundedFiles
							ReturnedData.Add(itm)
						Next
					Next                                                        'Process next FileFilter

				Catch ex As Exception                                           'For simplicity sake
				End Try                                                         'We'll ignore the errors
			Loop                                                                'Process next folder in the stack

			Return ReturnedData

		End Function


#Region "Helper class"

		Private Class DocScanLocalViewData
			Inherits SP.Main.Notify.SPScanJobWebService.ScanAttachmentDTO


		End Class

		Private Class NotScanedDocfiles
			Public Property CustomerID As String
			Public Property Filename As String
			Public Property CreatedOn As DateTime

		End Class


#End Region


	End Class

End Namespace
