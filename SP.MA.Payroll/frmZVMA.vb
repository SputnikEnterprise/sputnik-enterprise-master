Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.PayrollMng
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SPS.MA.Guthaben
Imports System.Reflection
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraPrinting
Imports DevExpress.LookAndFeel
Imports System.Drawing.Printing
Imports System.IO
Imports DevExpress.XtraEditors.Repository

Namespace UI

	Public Class frmZVMA

#Region "Private Fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The common database access.
		''' </summary>
		Private m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The Payroll data access object.
		''' </summary>
		Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_md As Mandant

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

		''' <summary>
		''' The Mandant number.
		''' </summary>
		Private m_MDNr As Integer
		Private m_SearchYear As Integer
		Private m_SearchMonth As Integer
		Private m_AssignedEmployees As List(Of Integer)


		Private printingSystem1 As New PrintingSystem()
		Private printableComponentLink1 As New PrintableComponentLink()


#End Region

#Region "Constructor"


		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal mdnr As Integer, ByVal year As Integer, ByVal month As Integer, ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			Dim currentDomain As AppDomain = AppDomain.CurrentDomain
			AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

			m_MDNr = mdnr
			m_SearchYear = year
			m_SearchMonth = month

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try
				m_md = New Mandant
				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			Dim conStr = m_md.GetSelectedMDData(mdnr).MDDbConn
			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.



			rtfContent.Unit = DevExpress.Office.DocumentUnit.Centimeter
			rtfContent.Document.Sections(0).Page.PaperKind = Printing.PaperKind.A4
			rtfContent.Text = String.Empty
			rtfContent.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden

			rtfContent.Font = New System.Drawing.Font("Calibri", 10, FontStyle.Regular)
			rtfContent.Document.DefaultCharacterProperties.FontName = "Calibri"
			rtfContent.Document.DefaultCharacterProperties.FontSize = 10
			rtfContent.ReadOnly = True

			rtfContent.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple
			rtfContent.Views.SimpleView.Padding = New DevExpress.Portable.PortablePadding(0)
			rtfContent.Document.Sections(0).Margins.Left = 0
			rtfContent.Document.Sections(0).Margins.Right = 0
			rtfContent.Document.Sections(0).Margins.Top = 0
			rtfContent.Document.Sections(0).Margins.Bottom = 0
			rtfContent.ReadOnly = True


			Reset()
			' Translate controls.
			TranslateControls()

		End Sub

#End Region

#Region "Private Properties"


		''' <summary>
		''' Gets the selected employee data.
		''' </summary>
		''' <returns>The selected employee data or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedZVARGBEmployeeData As EmployeeDataForZV
			Get
				Dim grdView = TryCast(grdZVARGBEmployees.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim employeeDataForZV = CType(grdView.GetRow(selectedRows(0)), EmployeeDataForZV)
						Return employeeDataForZV
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets the selected data.
		''' </summary>
		''' <returns>The selected employee data or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedSuspectData As SuspectPayrollData
			Get
				Dim grdView = TryCast(grdSuspectPayroll.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim data = CType(grdView.GetRow(selectedRows(0)), SuspectPayrollData)
						Return data
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets the selected data.
		''' </summary>
		''' <returns>The selected employee data or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedPrintCheckCashData As PayrollCheckCashData
			Get
				Dim grdView = TryCast(grdCheckCash.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim data = CType(grdView.GetRow(selectedRows(0)), PayrollCheckCashData)
						Return data
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region


#Region "Public Methods"

		''' <summary>
		''' Loads the data.
		''' </summary>
		''' <param name="employeeNumbers">The employee numbers to load.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadData(ByVal payrollsNumbers As Integer(), ByVal employeeNumbers As Integer()) As Boolean

			Dim success As Boolean = True

			m_AssignedEmployees = employeeNumbers.ToList

			success = success AndAlso LoadPayrolProtocollData()
			success = success AndAlso LoadEmployeeDataForZV(m_AssignedEmployees)

			If Not payrollsNumbers Is Nothing Then
				success = success AndAlso LoadEmployeeDataForSupspectPayrolls(payrollsNumbers)
				success = success AndAlso LoadEmployeeDataForPrintCheckCash(payrollsNumbers)
			Else
				xtabSuspectPayrolls.PageEnabled = False
				xtabCheckCash.PageEnabled = False
			End If
			xtabSuspectPayrolls.PageEnabled = False


			Return success
		End Function

		''' <summary>
		''' Loads the zv and argb data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadZVARGBData() As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadEmployeeDataForZV(Nothing)

			xtabProtocoll.PageVisible = False
			xtabCheckCash.PageVisible = False
			xtabSuspectPayrolls.PageVisible = False


			Return success
		End Function


#End Region


#Region "Private Methods"

		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.lblHeadingText.Text = m_Translate.GetSafeTranslationValue(Me.lblHeadingText.Text)
			Me.lblHeadingSubText.Text = m_Translate.GetSafeTranslationValue(Me.lblHeadingSubText.Text)

			tgsSelectedEmployee.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSelectedEmployee.Properties.OffText)
			tgsSelectedEmployee.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSelectedEmployee.Properties.OnText)

			Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)
			Me.btnOpenZVForm.Text = m_Translate.GetSafeTranslationValue(Me.btnOpenZVForm.Text)
			Me.btnOpenAGForm.Text = m_Translate.GetSafeTranslationValue(Me.btnOpenAGForm.Text)

		End Sub

		''' <summary>
		''' Resets the form.
		''' </summary>
		Private Sub Reset()

			rtfContent.HtmlText = Nothing

			xtabZV.PageEnabled = False
			xtabCheckCash.PageEnabled = False
			xtabSuspectPayrolls.PageEnabled = False


			' ---Reset drop downs, grids and lists---
			ResetEmployeeGrid()
			ResetSuspectPayrollGrid()
			ResetPrintCheckCashGrid()

		End Sub


		''' <summary>
		''' Resets the employee data grid.
		''' </summary>
		Private Sub ResetEmployeeGrid()

			' Reset the grid

			gvZVARGBEmployees.OptionsView.ShowIndicator = False
			gvZVARGBEmployees.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvZVARGBEmployees.Columns.Clear()


			Dim checkEditReExisting As RepositoryItemCheckEdit
			checkEditReExisting = CType(grdZVARGBEmployees.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			checkEditReExisting.PictureChecked = My.Resources.Checked
			checkEditReExisting.PictureUnchecked = Nothing
			checkEditReExisting.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined



			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "MANr"
			columnMANr.FieldName = "MANr"
			columnMANr.Width = 50
			columnMANr.Visible = True
			gvZVARGBEmployees.Columns.Add(columnMANr)

			Dim columnLastName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLastName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLastName.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnLastName.Name = "EmployeeFullnameWithComma"
			columnLastName.FieldName = "EmployeeFullnameWithComma"
			columnLastName.Width = 200
			columnLastName.Visible = True
			gvZVARGBEmployees.Columns.Add(columnLastName)

			Dim columnZVPrinted As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZVPrinted.Caption = m_Translate.GetSafeTranslationValue("ZV.-Gedruckt?")
			columnZVPrinted.Name = "ZVPrinted"
			columnZVPrinted.FieldName = "ZVPrinted"
			columnZVPrinted.Width = 50
			columnZVPrinted.Visible = True
			columnZVPrinted.ColumnEdit = checkEditReExisting
			columnZVPrinted.UnboundType = DevExpress.Data.UnboundColumnType.Boolean
			gvZVARGBEmployees.Columns.Add(columnZVPrinted)

			Dim columnARGBPrinted As New DevExpress.XtraGrid.Columns.GridColumn()
			columnARGBPrinted.Caption = m_Translate.GetSafeTranslationValue("ARGB-Gdruckt?")
			columnARGBPrinted.Name = "ARGBPrinted"
			columnARGBPrinted.FieldName = "ARGBPrinted"
			columnARGBPrinted.Width = 50
			columnARGBPrinted.Visible = True
			columnARGBPrinted.ColumnEdit = checkEditReExisting
			columnARGBPrinted.UnboundType = DevExpress.Data.UnboundColumnType.Boolean
			gvZVARGBEmployees.Columns.Add(columnARGBPrinted)


			grdZVARGBEmployees.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the suspect payroll data grid.
		''' </summary>
		Private Sub ResetSuspectPayrollGrid()

			' Reset the grid

			gvSuspectPayroll.OptionsView.ShowIndicator = False
			gvSuspectPayroll.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvSuspectPayroll.Columns.Clear()

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "MANr"
			columnMANr.FieldName = "MANr"
			columnMANr.Visible = True
			columnMANr.Width = 10
			gvSuspectPayroll.Columns.Add(columnMANr)

			Dim columnEmployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeeFullname.Name = "EmployeeFullname"
			columnEmployeeFullname.FieldName = "EmployeeFullname"
			columnEmployeeFullname.Visible = True
			gvSuspectPayroll.Columns.Add(columnEmployeeFullname)

			grdSuspectPayroll.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the advanced payment for print check and cash data grid.
		''' </summary>
		Private Sub ResetPrintCheckCashGrid()

			' Reset the grid

			gvCheckCash.OptionsView.ShowIndicator = False
			gvCheckCash.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvCheckCash.Columns.Clear()

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "MANr"
			columnMANr.FieldName = "MANr"
			columnMANr.Visible = True
			columnMANr.Width = 10
			gvCheckCash.Columns.Add(columnMANr)

			Dim columnZGNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZGNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZGNr.Caption = m_Translate.GetSafeTranslationValue("Vorschuss-Nr.")
			columnZGNr.Name = "ZGNr"
			columnZGNr.FieldName = "ZGNr"
			columnZGNr.Visible = True
			columnZGNr.Width = 10
			gvCheckCash.Columns.Add(columnZGNr)

			Dim columnEmployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeeFullname.Name = "EmployeeFullname"
			columnEmployeeFullname.FieldName = "EmployeeFullname"
			columnEmployeeFullname.Visible = True
			gvCheckCash.Columns.Add(columnEmployeeFullname)

			Dim columnLALabel As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLALabel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLALabel.Caption = m_Translate.GetSafeTranslationValue("Zahlungsart")
			columnLALabel.Name = "LALabel"
			columnLALabel.FieldName = "LALabel"
			columnLALabel.Visible = True
			columnLALabel.Width = 20
			gvCheckCash.Columns.Add(columnLALabel)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "Betrag"
			columnBetrag.FieldName = "Betrag"
			columnBetrag.Visible = True
			'columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "n2"
			columnBetrag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom
			columnBetrag.SummaryItem.DisplayFormat = "{0:n2}"
			columnBetrag.Width = 10
			gvCheckCash.Columns.Add(columnBetrag)

			grdCheckCash.DataSource = Nothing

		End Sub


		''' <summary>
		''' Loads payroll protocoll.
		''' </summary>
		Private Function LoadPayrolProtocollData() As Boolean
			Dim result As Boolean = True

			Dim protocoldata = m_PayrollDatabaseAccess.LoadLOProtocol(m_MDNr, Nothing, m_SearchMonth, m_SearchYear)

			If protocoldata Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Protokoll für Lohnlauf konnte nicht erstellt werden."))
			End If

			Try
				btnSendProtocol.Enabled = (m_InitializationData.UserData.UserNr = 1) OrElse protocoldata.Protokoll.Contains("Error") OrElse protocoldata.Protokoll.Contains("Warn")
				rtfContent.HtmlText = protocoldata.Protokoll

			Catch ex As Exception
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Protokoll konnte nicht angezeigt werden."))
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads employee data for ZV.
		''' </summary>
		''' <param name="employeeNumbers">The employee numbers.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeDataForZV(ByVal employeeNumbers As List(Of Integer)) As Boolean

			Dim listOfEmployeeData As List(Of EmployeeDataForZV) = Nothing
			Dim listOfAssignedEmployeeData As New List(Of EmployeeDataForZV)

			listOfEmployeeData = m_PayrollDatabaseAccess.LoadEmployeesForForgottenZV(m_MDNr, m_SearchMonth, m_SearchYear, m_InitializationData.UserData.UserFullName)
			If listOfEmployeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterdaten für ZV und Arbeitgeberbescheinigung konnten nicht geladnen werden."))

				Return False
			End If

			'If employeeNumbers Is Nothing Then

			'	listOfEmployeeData = m_PayrollDatabaseAccess.LoadListOfAllKandidaten4NotCreatedZV(m_InitializationData.UserData.UserFName & " " & m_InitializationData.UserData.UserLName, m_MDNr)

			'Else
			'	listOfEmployeeData = m_PayrollDatabaseAccess.LoadEmployeeData4NotCreatedZV(employeeNumbers)

			'End If
			If tgsSelectedEmployee.EditValue Then
				For Each itm In listOfEmployeeData
					If employeeNumbers.Contains(itm.MANr) Then listOfAssignedEmployeeData.Add(itm)
				Next
				grdZVARGBEmployees.DataSource = listOfAssignedEmployeeData

			Else
				grdZVARGBEmployees.DataSource = listOfEmployeeData

			End If

			xtabZV.PageEnabled = gvZVARGBEmployees.RowCount > 0

			Return Not listOfEmployeeData Is Nothing
		End Function

		''' <summary>
		''' Loads employee data for suspect payroll.
		''' </summary>
		''' <param name="payrollNumbers">The employee numbers.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeDataForSupspectPayrolls(ByVal payrollNumbers As Integer()) As Boolean

			Dim listOfEmployeeData As List(Of SuspectPayrollData) = Nothing

			If Not payrollNumbers Is Nothing Then
				listOfEmployeeData = m_PayrollDatabaseAccess.LoadSuspectPayrollsAfterCreate(m_MDNr, payrollNumbers.ToArray())
			End If
			grdSuspectPayroll.DataSource = listOfEmployeeData

			If listOfEmployeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehlerhafte Daten konnten nicht geladnen werden."))
				Return False
			End If
			xtabSuspectPayrolls.PageEnabled = listOfEmployeeData.Count > 0

			Return Not listOfEmployeeData Is Nothing

		End Function

		''' <summary>
		''' Loads employee data for print check and cash.
		''' </summary>
		''' <param name="payrollNumbers">The employee numbers.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeDataForPrintCheckCash(ByVal payrollNumbers As Integer()) As Boolean

			Dim listOfEmployeeData As List(Of PayrollCheckCashData) = Nothing

			If Not payrollNumbers Is Nothing Then
				listOfEmployeeData = m_PayrollDatabaseAccess.LoadEmployeesForPrintCheckCashAfterPayroll(m_MDNr, payrollNumbers.ToArray())
			End If

			grdCheckCash.DataSource = listOfEmployeeData

			If listOfEmployeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Auszahlungsdaten für Check und Bar konnten nicht geladnen werden."))
				Return False
			End If
			xtabCheckCash.PageEnabled = listOfEmployeeData.Count > 0

			Return Not listOfEmployeeData Is Nothing

		End Function


#End Region

#Region "Event Handlers"

		''' <summary>
		''' Handles click on close button.
		''' </summary>
		Private Sub OnBtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			Close()
		End Sub

		''' <summary>
		'''Handles clik on btnOpenZVForm.
		''' </summary>
		Private Sub OnBtnOpenZVForm_Click(sender As Object, e As EventArgs) Handles btnOpenZVForm.Click
			Dim employeeData = SelectedZVARGBEmployeeData

			If Not employeeData Is Nothing Then

				Try
					Dim frmZV = New SPS.MA.Guthaben.frmZV(m_InitializationData)

					Dim preselectionSetting As New PreselectionZVData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = employeeData.MANr, .Year = m_SearchYear, .Month = m_SearchMonth}
					frmZV.PreselectionData = preselectionSetting

					frmZV.LoadData()
					frmZV.DisplayEmployeeData()

					frmZV.Show()
					frmZV.BringToFront()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))

				End Try

			End If

		End Sub

		Private Sub OngvZVARGBEmployees_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvZVARGBEmployees.CustomUnboundColumnData

			If e.Column.Name = "ARGBPrinted" Then
				If (e.IsGetData()) Then
					Dim data = CType(e.Row, EmployeeDataForZV)

					e.Value = If(data.ARGBPrinted.GetValueOrDefault(False), My.Resources.Checked, Nothing)
				End If
			End If

		End Sub


		''' <summary>
		'''Handles clik on btnOpenAGForm.
		''' </summary>
		Private Sub OnBtnOpenAGForm_Click(sender As Object, e As EventArgs) Handles btnOpenAGForm.Click
			Dim employeeData = SelectedZVARGBEmployeeData

			If Not employeeData Is Nothing Then

				Try

					Dim frmARGB = New SPS.MA.Guthaben.frmARGB(m_InitializationData)

					Dim preselectionSetting As New SPS.MA.Guthaben.PreselectionARGBData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = employeeData.MANr}
					frmARGB.PreselectionData = preselectionSetting

					frmARGB.LoadData()
					frmARGB.DisplayEmployeeData()

					frmARGB.Show()
					frmARGB.BringToFront()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))

				End Try

			End If

		End Sub

		Private Sub OnBtnPrintProtocol_Click(sender As Object, e As EventArgs) Handles btnPrintProtocol.Click
			PreviewPrintableComponent(rtfContent, UserLookAndFeel.Default)
		End Sub

		Private Sub PreviewPrintableComponent(component As IPrintable, lookAndFeel As UserLookAndFeel)
			' Create a link that will print a control.  
			Dim link As New PrintableComponentLink() With {
						.PrintingSystemBase = New PrintingSystem(),
						.Component = component,
						.Landscape = True,
						.PaperKind = PaperKind.A4,
						.Margins = New Margins(10, 10, 10, 10)
				}
			' Show the report. 
			link.ShowRibbonPreview(lookAndFeel)

		End Sub

		Private Sub OnBtnSendProtocol_Click(sender As Object, e As EventArgs) Handles btnSendProtocol.Click
			Dim msg As String = "Der Versand wurde erfolgreich abgeschlossen"
			Dim docFileName As String = Path.Combine(m_InitializationData.UserData.spAllowedPath, Path.ChangeExtension(Path.GetRandomFileName, "html"))

			Dim body As String = String.Format("Mandant: {0} ({1})<br>", m_InitializationData.MDData.MDName, m_InitializationData.MDData.MDCity)
			body &= String.Format("Benutzer: {0} ({1})", m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserMDeMail)

			rtfContent.SaveDocument(docFileName, DevExpress.XtraRichEdit.DocumentFormat.Html)
			Dim result = m_UtilityUI.SendMailNotification("Protokoll für Lohnabrechnung", body, "", New List(Of String) From {docFileName})

			If result Then m_UtilityUI.ShowInfoDialog(msg) Else m_UtilityUI.ShowErrorDialog("Der Versand wurde nicht erfolgreich abgeschlossen")

			Try
				File.Delete(docFileName)
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

		End Sub

		Private Sub l_CreateDetailArea(ByVal sender As Object, ByVal e As CreateAreaEventArgs)
			Dim g As Graphics = Graphics.FromHwnd(IntPtr.Zero)
			Dim sz As SizeF = g.MeasureString(rtfContent.HtmlText, rtfContent.Font, rtfContent.Width)
			Dim tb As New TextBrick(BorderSide.None, 0, Color.Red, rtfContent.BackColor, rtfContent.ForeColor)

			tb.Rect = New RectangleF(New PointF(0, 0), sz)
			tb.Font = rtfContent.Font
			tb.Text = rtfContent.Text

			e.Graph.DrawBrick(tb)

		End Sub


		Private Sub OngvZVARGBEmployees_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvZVARGBEmployees.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim employeeData = SelectedZVARGBEmployeeData

				' Send a request to open a employeeMng form.
				Dim hub = MessageService.Instance.Hub
				Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, employeeData.MANr)
				hub.Publish(openEmployeeMng)

			End If

		End Sub

		''' <summary>
		''' Handles cell click on available suspect payroll grid.
		''' </summary>
		Private Sub OnGvSuspectPayroll_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvSuspectPayroll.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim employeeData = SelectedSuspectData

				' Send a request to open a employeeMng form.
				Dim hub = MessageService.Instance.Hub
				Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, employeeData.MANr)
				hub.Publish(openEmployeeMng)

			End If

		End Sub

		''' <summary>
		''' Handles cell click on available check and cash grid.
		''' </summary>
		Private Sub OnGvCheckCash_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvCheckCash.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim employeeData = SelectedPrintCheckCashData

				Select Case column.Name.ToLower
					Case "MANr".ToLower, "Nachname_Vorname".ToLower

						' Send a request to open a employeeMng form.
						Dim hub = MessageService.Instance.Hub
						Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, employeeData.MANr)
						hub.Publish(openEmployeeMng)


					Case Else
						' Send a request to open a advancedpaymentMng form.
						Dim hub = MessageService.Instance.Hub
						Dim openAdvancePaymentMng As New OpenAdvancePaymentMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, employeeData.ZGNr)
						hub.Publish(openAdvancePaymentMng)

				End Select

			End If

		End Sub

		Private Sub tgsSelectedEmployee_Toggled(sender As Object, e As EventArgs) Handles tgsSelectedEmployee.Toggled
			If m_SuppressUIEvents Then Return
			LoadEmployeeDataForZV(m_AssignedEmployees)
		End Sub

#End Region



#Region "helpers"

		Private Shared Function MyResolveEventHandler(sender As Object, args As ResolveEventArgs) As Assembly
			'This handler is called only when the common language runtime tries to bind to the assembly and fails.        
			m_Logger = New Logger

			'Retrieve the list of referenced assemblies in an array of AssemblyName.
			Dim objExecutingAssemblies As [Assembly]
			objExecutingAssemblies = [Assembly].GetExecutingAssembly()
			Dim arrReferencedAssmbNames() As AssemblyName
			arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies()

			'Loop through the array of referenced assembly names.
			Dim strAssmbName As AssemblyName
			For Each strAssmbName In arrReferencedAssmbNames

				'Look for the assembly names that have raised the "AssemblyResolve" event.
				If (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) = args.Name.Substring(0, args.Name.IndexOf(","))) Then

					'Build the path of the assembly from where it has to be loaded.
					Dim strTempAssmbPath As String = String.Empty
					strTempAssmbPath = IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), args.Name.Substring(0, args.Name.IndexOf(",")) & ".dll")

					If IO.File.Exists(strTempAssmbPath) Then
						Dim msg = String.Format("loading Assembly: {0}", strTempAssmbPath)
						m_Logger.LogWarning(msg)
						Trace.WriteLine(String.Format("loading Assembly: ", strTempAssmbPath))
						Dim MyAssembly As [Assembly]

						'Load the assembly from the specified path. 
						MyAssembly = [Assembly].LoadFrom(strTempAssmbPath)

						'Return the loaded assembly.
						Return MyAssembly
					Else
						Dim msg = String.Format("Assembly could not be found: {0}", strTempAssmbPath)
						m_Logger.LogWarning(msg)
						Trace.WriteLine(msg)
					End If

				End If
			Next

			Return Nothing

		End Function





#End Region

	End Class

End Namespace
