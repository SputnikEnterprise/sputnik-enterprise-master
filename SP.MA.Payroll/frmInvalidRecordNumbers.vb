Imports SP.DatabaseAccess.PayrollMng
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports SPProgUtility.Mandanten

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

      Dim conStr = m_md.GetSelectedMDData(mdnr).MDDbConn
      m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

      m_UtilityUI = New UtilityUI
      m_Utility = New Utility

      ' Translate controls.
      TranslateControls()

      Reset()
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Loads the data.
    ''' </summary>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="maNr">The employee number.</param>
    ''' <param name="month">The month.</param>
    ''' <param name="year">The year.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function LoadData(mdNr As Integer, maNr As Integer, month As Integer, year As Integer) As Boolean

      Dim success As Boolean = True

      Dim data = m_PayrollDatabaseAccess.LoadInvalidRecordNumbersForPayroll(mdNr, maNr, month, year)

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
			gvInvalidRecordNumbers.OptionsView.ShowIndicator = False
			gvInvalidRecordNumbers.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvInvalidRecordNumbers.OptionsView.ColumnAutoWidth = False
			gvInvalidRecordNumbers.OptionsView.ColumnAutoWidth = False
			gvInvalidRecordNumbers.OptionsView.ShowAutoFilterRow = False

			gvInvalidRecordNumbers.Columns.Clear()


			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Modul")
			columnMANr.Name = "ModuleName"
			columnMANr.FieldName = "ModuleName"
			columnMANr.Visible = True
			columnMANr.Width = 100
			gvInvalidRecordNumbers.Columns.Add(columnMANr)

			Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnRecordNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnRecordNumber.AppearanceHeader.Options.UseTextOptions = True
			columnRecordNumber.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnRecordNumber.AppearanceCell.Options.UseTextOptions = True
			columnRecordNumber.Name = "RecordNumber"
			columnRecordNumber.FieldName = "RecordNumber"
			columnRecordNumber.Visible = True
			columnRecordNumber.BestFit()
			gvInvalidRecordNumbers.Columns.Add(columnRecordNumber)

		End Sub

#End Region

#Region "Event Handling"

		''' <summary>
		''' Handles double click on grid.
		''' </summary>
		Private Sub OnGrdInvalidRecordNumbers_DoubleClick(sender As System.Object, e As System.EventArgs) Handles grdInvalidRecordNumbers.DoubleClick
			Dim selectedRows = gvInvalidRecordNumbers.GetSelectedRows()

			If (selectedRows.Count > 0) Then
				Dim modulRecordNumberData = CType(gvInvalidRecordNumbers.GetRow(selectedRows(0)), ModuleRecordNumber)

				Dim hub = MessageService.Instance.Hub

				Select Case modulRecordNumberData.ModuleName

					Case "LO"

					Case "RP"
						Dim openReportsMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, modulRecordNumberData.RecordNumber)
						hub.Publish(openReportsMng)

					Case "ES"
						Dim openReportsMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, modulRecordNumberData.RecordNumber)
						hub.Publish(openReportsMng)

					Case "MA"
						Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, modulRecordNumberData.RecordNumber)
						hub.Publish(openEmployeeMng)


					Case Else
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ungülter Modulname"))

				End Select

			End If
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