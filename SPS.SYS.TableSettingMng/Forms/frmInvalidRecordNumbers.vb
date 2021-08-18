

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports SPProgUtility.Mandanten
Imports SP
Imports DevExpress.XtraGrid.Views.Grid



Namespace UI



	Public Class frmInvalidRecordNumbers

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
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

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
		''' The mandant number.
		''' </summary>
		Private m_MDNr As Integer

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal mdnr As Integer, ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try
				m_MDNr = mdnr
				m_md = New Mandant
				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			' Translate controls.
			TranslateControls()

			AddHandler gvInvalidRecordNumbers.RowCellClick, AddressOf Ongv_RowCellClick

			Reset()

		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads the data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadData(ByVal mdNr As Integer, year As Integer, month As Integer) As Boolean

			Dim success As Boolean = True

			Dim data = m_CommonDatabaseAccess.LoadNotInvalidDataForClosingMonth(mdNr, year, month)

			grdInvalidRecordNumbers.DataSource = data

			Return Not data Is Nothing
		End Function

#End Region

#Region "Reset"

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()

			m_SuppressUIEvents = True

			' ---Reset drop downs, grids and lists---
			ResetInvalidRecordNumbersGrid()

			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		''' Resets invalid record numbers grid. 
		''' </summary>
		Private Sub ResetInvalidRecordNumbersGrid()

			' Reset the grid
			gvInvalidRecordNumbers.FocusRectStyle = DrawFocusRectStyle.RowFocus
			gvInvalidRecordNumbers.OptionsView.ShowIndicator = True
			gvInvalidRecordNumbers.OptionsView.ShowAutoFilterRow = True

			'gvInvalidRecordNumbers.OptionsSelection.EnableAppearanceFocusedRow = False
			gvInvalidRecordNumbers.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvInvalidRecordNumbers.Columns.Clear()

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Modul")
			columnmodulname.Name = "modulname"
			columnmodulname.FieldName = "modulname"
			columnmodulname.Visible = True
			columnmodulname.Width = 20
			gvInvalidRecordNumbers.Columns.Add(columnmodulname)

			Dim columnmodulnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulnr.Caption = m_Translate.GetSafeTranslationValue("Modulnummer")
			columnmodulnr.Name = "modulnr"
			columnmodulnr.FieldName = "modulnr"
			columnmodulnr.Visible = True
			columnmodulnr.Width = 30
			gvInvalidRecordNumbers.Columns.Add(columnmodulnr)

			Dim columnmaname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmaname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnmaname.Name = "maname"
			columnmaname.FieldName = "maname"
			columnmaname.Visible = True
			'columnmaname.BestFit()
			gvInvalidRecordNumbers.Columns.Add(columnmaname)

			Dim columnmanr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmanr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnmanr.Name = "manr"
			columnmanr.FieldName = "manr"
			columnmanr.Visible = False
			'columnmanr.BestFit()
			gvInvalidRecordNumbers.Columns.Add(columnmanr)

			Dim columnStartdatum As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStartdatum.Caption = m_Translate.GetSafeTranslationValue("Startdatum")
			columnStartdatum.Name = "startdate"
			columnStartdatum.FieldName = "startdate"
			columnStartdatum.Visible = True
			columnStartdatum.Width = 30
			gvInvalidRecordNumbers.Columns.Add(columnStartdatum)

			grdInvalidRecordNumbers.DataSource = Nothing

		End Sub

#End Region


#Region "Event Handling"

		''' <summary>
		'''  Handles RowStyle event of lueBranch grid view.
		''' </summary>
		Private Sub OnGvLueBranch_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvInvalidRecordNumbers.RowStyle

			If e.RowHandle >= 0 Then

				Dim rowData = CType(gvInvalidRecordNumbers.GetRow(e.RowHandle), NotValidatedData)

				If rowData.modulname = "ZG" Then
					e.Appearance.BackColor = Color.Gainsboro
					e.Appearance.BackColor2 = Color.Gainsboro
				End If

			End If

		End Sub

		''' <summary>
		''' Handles cell click on grid.
		''' </summary>
		Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvInvalidRecordNumbers.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then
					Dim viewData = CType(dataRow, NotValidatedData)

					Select Case column.Name.ToLower
						Case "maname", "manr"
							If viewData.manr.HasValue Then OpenSelectedEmployee(viewData.manr)


						Case Else
							If viewData.modulname.ToLower = "rp" Then
								OpenSelectedReport(viewData.modulnr)

							ElseIf viewData.modulname.ToLower = "zg" Then
								OpenSelectedAdvancePayment(viewData.modulnr)

							End If

					End Select

				End If

			End If

		End Sub

		Private Sub OpenSelectedEmployee(ByVal employeeNumber As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employeeNumber)
				hub.Publish(openMng)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub

		Private Sub OpenSelectedReport(ByVal reportNumber As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openReportsMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, reportNumber)
				hub.Publish(openReportsMng)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub

		Private Sub OpenSelectedAdvancePayment(ByVal advancePaymentNumber As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openAdvancePaymentMng As New OpenAdvancePaymentMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, advancePaymentNumber)
				hub.Publish(openAdvancePaymentMng)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub



#End Region


#Region "Helper Methods"

		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.lblInvalidRecords.Text = m_Translate.GetSafeTranslationValue(Me.lblInvalidRecords.Text)

		End Sub

#End Region


	End Class


End Namespace