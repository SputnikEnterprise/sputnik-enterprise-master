
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ColorUtility.ClsColorUtility

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports SP.DatabaseAccess.AdvancePaymentMng
Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects

Public Class frmNotDeletedZG

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_ZGDatabaseAccess As IAdvancePaymentDatabaseAccess

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI
	Private m_utility As SPProgUtility.MainUtilities.Utilities

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()
	Private m_md As Mandant

	'Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private Property _LiNotDeletedZGNr As New List(Of Integer)
	Private Property _iNotDeletedZGNr As Integer


#Region "Construct"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _LiZGNr As List(Of Integer))

		m_md = New Mandant
		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_UtilityUI = New UtilityUI
		m_utility = New SPProgUtility.MainUtilities.Utilities

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		Dim connectionString As String = m_InitializationData.MDData.MDDbConn
		m_ZGDatabaseAccess = New AdvancePaymentDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		_LiNotDeletedZGNr = _LiZGNr

		TranslateControls()

		ResetAdvancePaymentGrid()
		LoadCustomerContactList()

		AddHandler gvExistingAdvancePayment.RowCellClick, AddressOf Ongv_RowCellClick

	End Sub

#End Region


	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

	End Sub

	Private Sub ResetAdvancePaymentGrid()

		' Reset the grid
		gvExistingAdvancePayment.Columns.Clear()
		gvExistingAdvancePayment.OptionsView.ShowAutoFilterRow = True

		Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnrecid.Name = "ID"
		columnrecid.FieldName = "ID"
		columnrecid.Visible = False
		gvExistingAdvancePayment.Columns.Add(columnrecid)

		Dim columnzgnr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzgnr.Caption = m_Translate.GetSafeTranslationValue("Vorschuss-Nr.")
		columnzgnr.Name = "ZGNr"
		columnzgnr.FieldName = "ZGNr"
		columnzgnr.Visible = True
		gvExistingAdvancePayment.Columns.Add(columnzgnr)

		Dim columnmanr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmanr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnmanr.Name = "MANR"
		columnmanr.FieldName = "MANR"
		columnmanr.Visible = False
		gvExistingAdvancePayment.Columns.Add(columnmanr)

		Dim columnLastnameFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastnameFirstname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnLastnameFirstname.Name = "EmployeeFullnameWithComma"
		columnLastnameFirstname.FieldName = "EmployeeFullnameWithComma"
		columnLastnameFirstname.Visible = True
		gvExistingAdvancePayment.Columns.Add(columnLastnameFirstname)

		Dim columnaus_dat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnaus_dat.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnaus_dat.Name = "Aus_Dat"
		columnaus_dat.FieldName = "Aus_Dat"
		columnaus_dat.Visible = True
		gvExistingAdvancePayment.Columns.Add(columnaus_dat)

		Dim columnzeitraum As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzeitraum.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
		columnzeitraum.Name = "Period"
		columnzeitraum.FieldName = "Period"
		columnzeitraum.Visible = True
		gvExistingAdvancePayment.Columns.Add(columnzeitraum)

		Dim columnbetrag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnbetrag.Name = "Betrag"
		columnbetrag.FieldName = "Betrag"
		columnbetrag.Visible = True
		columnbetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbetrag.DisplayFormat.FormatString = "N2"
		gvExistingAdvancePayment.Columns.Add(columnbetrag)

		Dim columnZGGrund As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZGGrund.Caption = m_Translate.GetSafeTranslationValue("Grund")
		columnZGGrund.Name = "ZGGRUND"
		columnZGGrund.FieldName = "ZGGRUND"
		columnZGGrund.Visible = True
		gvExistingAdvancePayment.Columns.Add(columnZGGrund)


		gridExistingAdvancePayment.DataSource = Nothing

	End Sub

	Private Sub frmNotDeletedZG_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		If Not Me.WindowState = System.Windows.Forms.FormWindowState.Maximized Then
			My.Settings.iHeight = Me.Height
			My.Settings.iWidth = Me.Width
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.Save()
		End If

	End Sub

	Private Sub frmNotDeletedZG_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			If My.Settings.iHeight > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.iHeight)
			If My.Settings.iWidth > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.iWidth)
			If My.Settings.frmLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
				If System.Windows.Forms.Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > System.Windows.Forms.Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Function LoadCustomerContactList() As Boolean

		Dim listOfEmployees = GetAdvancePaymentDbData4Delete()
		If listOfEmployees Is Nothing Then Return False


		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New ZGMasterData With
																						 {.ID = person.ID,
																							.ZGNr = person.ZGNr,
																							.MANR = person.MANR,
																							.EmployeeLastname = person.EmployeeLastname,
																							.EmployeeFirstname = person.EmployeeFirstname,
																							.ZGGRUND = person.ZGGRUND,
																							.LP = person.LP,
																							.JAHR = person.JAHR,
																							.Aus_Dat = person.Aus_Dat,
																							.Betrag = person.Betrag
																						}).ToList()

		Dim listDataSource As BindingList(Of ZGMasterData) = New BindingList(Of ZGMasterData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		gridExistingAdvancePayment.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing

	End Function

	Function GetAdvancePaymentDbData4Delete() As IEnumerable(Of ZGMasterData)

		Dim data = m_ZGDatabaseAccess.LoadAssignedZGMasterData(_LiNotDeletedZGNr)
		If data Is Nothing OrElse data.Count = 0 Then
			Dim msg As String = "Keine Vorschuss Daten wurden gefunden."
			m_UtilityUI.ShowErrorDialog(msg)

			Return Nothing
		End If


		Return data

	End Function


	'Dim sql As String

	'Dim zgNumbersBuffer As String = String.Empty

	'For Each number In _LiNotDeletedZGNr

	'	zgNumbersBuffer = zgNumbersBuffer & IIf(zgNumbersBuffer <> "", ", ", "") & number

	'Next

	'sql = "SELECT ZG.ID, ZG.ZGNr, ZG.MANr, ZG.ZGGrund, (ZG.Betrag * (-1)) As Betrag, ZG.Aus_Dat, "
	'sql += "Convert(int, ZG.LP) As LP, Convert(int, ZG.Jahr) As Jahr, "
	'sql += "MA.Nachname, MA.Vorname "
	'sql += "FROM ZG "
	'sql += "Left Join Mitarbeiter MA On ZG.MANr = MA.MANr "
	'If Not String.IsNullOrWhiteSpace(zgNumbersBuffer) Then zgNumbersBuffer = String.Format("Where ZG.ZGNr In ({0}) ", zgNumbersBuffer)
	'sql = String.Format("{0} {1} Order By ZG.ZGNr Desc, MA.Nachname ASC, MA.Vorname ASC", sql, zgNumbersBuffer)

	'Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitializationData.MDData.MDDbConn, sql, Nothing)

	'Try

	'	If (Not reader Is Nothing) Then

	'		result = New List(Of FoundedAdvancePaymentData)

	'		While reader.Read()
	'			Dim overviewData As New FoundedAdvancePaymentData

	'			overviewData.recid = CInt(m_utility.SafeGetInteger(reader, "id", 0))
	'			overviewData.zgnr = CInt(m_utility.SafeGetInteger(reader, "zgnr", 0))
	'			overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "manr", 0))

	'			overviewData.employeelastname = m_utility.SafeGetString(reader, "Nachname")
	'			overviewData.employeefirstname = m_utility.SafeGetString(reader, "Vorname")

	'			overviewData.lp = m_utility.SafeGetInteger(reader, "LP", 0)
	'			overviewData.jahr = m_utility.SafeGetInteger(reader, "Jahr", 0)
	'			overviewData.betrag = m_utility.SafeGetDecimal(reader, "Betrag", 0)
	'			overviewData.Aus_Dat = m_utility.SafeGetDateTime(reader, "Aus_Dat", Nothing)
	'			overviewData.ZGGrund = m_utility.SafeGetString(reader, "ZGGrund")

	'			result.Add(overviewData)

	'		End While

	'	End If

	'Catch e As Exception
	'	result = Nothing
	'	m_Logger.LogError(e.ToString())

	'Finally
	'	m_utility.CloseReader(reader)

	'End Try


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvExistingAdvancePayment.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, ZGMasterData)

				Select Case column.Name.ToLower
					Case "manr", "EmployeeFullnameWithComma".ToLower
						If viewData.MANr > 0 Then loadSelectedEmployee(viewData.MANr)

					Case Else
						If viewData.zgnr > 0 Then loadSelectedAdvacePayment(viewData.zgnr)

				End Select

			End If

		End If

	End Sub


	Private Sub loadSelectedEmployee(ByVal employeeNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employeeNumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try


	End Sub

	Private Sub loadSelectedAdvacePayment(ByVal advancePaymentNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenAdvancePaymentMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, advancePaymentNumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

End Class