
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.ComponentModel
Imports DevExpress.LookAndFeel

Imports SP.Infrastructure.Logging

Imports SP.Infrastructure.UI

Imports SP.MA.AdvancePaymentMng
Imports SP.MA.AdvancePaymentMng.UI

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


Public Class frmZGSearch_LV
  Inherits DevExpress.XtraEditors.XtraForm
	Private m_Logger As ILogger = New Logger()

  Dim _ClsFunc As New ClsDivFunc

	Private m_Utility_SP As SPProgUtility.MainUtilities.Utilities

	Public Property RecCount As Integer
	Private Property Sql2Open As String

	Dim strLastSortBez As String

#Region "Diverse Speichermethoden..."

	'// Betrag inkl. MwSt.
	Dim _dBetrag_1 As Double
	Public Property GetTotal_1() As Double
		Get
			Return _dBetrag_1
		End Get
		Set(ByVal value As Double)
			If value < 0 Then
				_dBetrag_1 = value * CInt(IIf(CBool(ClsDataDetail.ShowBetragAsPositiv), -1, 1))
			Else
				_dBetrag_1 = value
			End If
		End Set
	End Property

#End Region

	Public Sub New(ByVal strQuery As String, ByVal LX As Integer, ByVal LY As Integer, ByVal lHeight As Integer)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()
		m_Utility_SP = New Utilities

		_dBetrag_1 = 0

		_ClsFunc.GetSearchQuery = strQuery
		Me.Sql2Open = strQuery

		ResetGridRPData()

	End Sub


	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As FoundedData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Function GetDbData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing
		m_Utility_SP = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_Utility_SP.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.ZGNr = CInt(m_Utility_SP.SafeGetInteger(reader, "ZGNr", 0))
					overviewData.RPNr = CInt(m_Utility_SP.SafeGetInteger(reader, "RPNr", 0))
					overviewData.MANr = CInt(m_Utility_SP.SafeGetInteger(reader, "MANr", 0))
					overviewData.LANr = CInt(m_Utility_SP.SafeGetInteger(reader, "LANr", 0))
					overviewData.LONr = CInt(m_Utility_SP.SafeGetInteger(reader, "LONr", 0))
					overviewData.VGNr = CInt(m_Utility_SP.SafeGetInteger(reader, "VGNr", 0))

					overviewData.employeename = String.Format("{0}, {1}", m_Utility_SP.SafeGetString(reader, "Nachname"),
																										m_Utility_SP.SafeGetString(reader, "Vorname"))

					overviewData.betrag = CDec(m_Utility_SP.SafeGetDecimal(reader, "betrag", 0))
					overviewData.monat = CInt(m_Utility_SP.SafeGetInteger(reader, "LP", 0))
					overviewData.jahr = CInt(m_Utility_SP.SafeGetInteger(reader, "Jahr", 0))

					overviewData.datum = CDate(m_Utility_SP.SafeGetDateTime(reader, "aus_dat", Nothing))
					overviewData.ersteller = String.Format("{0}, {1}", m_Utility_SP.SafeGetDateTime(reader, "createdon", Nothing),
																								 m_Utility_SP.SafeGetString(reader, "createdfrom"))
					overviewData.zggrund = m_Utility_SP.SafeGetString(reader, "zggrund")

					_dBetrag_1 += overviewData.betrag

					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_Utility_SP.CloseReader(reader)

		End Try

    Return result
  End Function

  Private Function LoadFoundedList() As Boolean

    Dim listOfEmployees = GetDbData4Show()

    Dim responsiblePersonsGridData = (From person In listOfEmployees
    Select New FoundedData With
           {.ZGNr = person.ZGNr,
            .MANr = person.MANr,
            .employeename = person.employeename,
            .monat = person.monat,
            .jahr = person.jahr,
            .LANr = person.LANr,
            .betrag = person.betrag,
            .datum = person.datum,
            .zggrund = person.zggrund,
            .ersteller = person.ersteller}).ToList()

    Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

    For Each p In responsiblePersonsGridData
      listDataSource.Add(p)
    Next

    grdRP.DataSource = listDataSource

    Return Not listOfEmployees Is Nothing
  End Function


  Private Sub ResetGridRPData()

    gvRP.OptionsView.ShowIndicator = False
    gvRP.OptionsView.ShowAutoFilterRow = True
    gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

    gvRP.Columns.Clear()

    Dim columnZGNr As New DevExpress.XtraGrid.Columns.GridColumn()
    columnZGNr.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue("ZGNr")
    columnZGNr.Name = "ZGNr"
    columnZGNr.FieldName = "ZGNr"
    columnZGNr.Visible = True
    gvRP.Columns.Add(columnZGNr)

    Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
    columnZGNr.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
    columnZGNr.Name = "MANr"
    columnZGNr.FieldName = "MANr"
    columnZGNr.Visible = False
    gvRP.Columns.Add(columnZGNr)

    Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
    columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    columnEmployee.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue("Kandidat")
    columnEmployee.Name = "employeename"
    columnEmployee.FieldName = "employeename"
    columnEmployee.Visible = True
    columnEmployee.BestFit()
    gvRP.Columns.Add(columnEmployee)

    Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
    columnMonth.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue("Monat")
    columnMonth.Name = "monat"
    columnMonth.FieldName = "monat"
    columnMonth.Visible = True
    columnMonth.BestFit()
    gvRP.Columns.Add(columnMonth)

    Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
    columnYear.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue("Jahr")
    columnYear.Name = "jahr"
    columnYear.FieldName = "jahr"
    columnYear.Visible = True
    columnYear.BestFit()
    gvRP.Columns.Add(columnYear)

    Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
    columnBetrag.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue("Betrag")
    columnBetrag.Name = "betrag"
    columnBetrag.FieldName = "betrag"
    columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    columnBetrag.DisplayFormat.FormatString = "n2"
    columnBetrag.Visible = True
    gvRP.Columns.Add(columnBetrag)

    Dim Columndatum As New DevExpress.XtraGrid.Columns.GridColumn()
    Columndatum.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue("Auszahlung am")
    Columndatum.Name = "datum"
    Columndatum.FieldName = "datum"
    Columndatum.Visible = True
    gvRP.Columns.Add(Columndatum)

    Dim ColumnsGrund As New DevExpress.XtraGrid.Columns.GridColumn()
    ColumnsGrund.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    ColumnsGrund.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue("Grund")
    ColumnsGrund.Name = "zggrund"
    ColumnsGrund.FieldName = "zggrund"
    ColumnsGrund.Visible = True
    gvRP.Columns.Add(ColumnsGrund)

    Dim ColumnErsteller As New DevExpress.XtraGrid.Columns.GridColumn()
    ColumnErsteller.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    ColumnErsteller.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue("Erstellt durch")
    ColumnErsteller.Name = "ersteller"
    ColumnErsteller.FieldName = "ersteller"
    ColumnErsteller.Visible = True
    gvRP.Columns.Add(ColumnErsteller)


    grdRP.DataSource = Nothing

  End Sub




	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = ClsDataDetail.m_Translate.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			Me.bsiTotalbetrag.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue(Me.bsiTotalbetrag.Caption)
			bbiPrintList.Caption = ClsDataDetail.m_Translate.GetSafeTranslationValue(Me.bbiPrintList.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmZGSearch_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_LVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmLVHeight = Me.Height
				My.Settings.ifrmLVWidth = Me.Width

				My.Settings.Save()
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		TranslateControls()

		Try
			Me.Width = Math.Max(My.Settings.ifrmLVWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmLVHeight, Me.Height)

			If My.Settings.frm_LVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_LVLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			LoadFoundedList()

			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format(ClsDataDetail.m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.RecCount)
			Me.bsiTotalbetrag.Caption = String.Format(ClsDataDetail.m_Translate.GetSafeTranslationValue("Totalbetrag: {0}"), Format(Me._dBetrag_1, "n2"))

			'If Me.RecCount > 0 Then CreateExportPopupMenu()
			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick

		Catch ex As Exception

		End Try

	End Sub

  Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

    If (e.Clicks = 2) Then

      Dim column = e.Column
      Dim dataRow = gvRP.GetRow(e.RowHandle)
      If Not dataRow Is Nothing Then
        Dim viewData = CType(dataRow, FoundedData)

        Select Case column.Name.ToLower
          Case "employeename"
            If viewData.MANr > 0 Then loadSelectedEmployee(viewData.MANr)

          Case Else
            If viewData.ZGNr > 0 Then loadSelectedzg(viewData.ZGNr)

        End Select

      End If

    End If

  End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub


#Region "Helpers..."

  Function GetMenuItems4Currencies(ByVal dBetrag_1 As Double) As List(Of String)
    Dim liResult As New List(Of String)

    Try
      liResult.Add(String.Format(ClsDataDetail.m_Translate.GetSafeTranslationValue("Totalbetrag: {0}"), Format(dBetrag_1, "c2")))

    Catch e As Exception
      MsgBox(Err.GetException.ToString)

    Finally

    End Try

    Return liResult

  End Function

	Sub loadSelectedEmployee(ByVal Employeenumber As Integer)
		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenEmployeeMngRequest(Me, ClsDataDetail.m_InitialData.UserData.UserNr, ClsDataDetail.m_InitialData.MDData.MDNr, Employeenumber)
		hub.Publish(openMng)

		'RunOpenMAForm(Employeenumber)
	End Sub

  Sub loadSelectedZG(ByVal iZGNr As Integer)
		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenAdvancePaymentMngRequest(Me, ClsDataDetail.m_InitialData.UserData.UserNr, ClsDataDetail.m_InitialData.MDData.MDNr, iZGNr)
		hub.Publish(openMng)
	End Sub



#End Region


End Class