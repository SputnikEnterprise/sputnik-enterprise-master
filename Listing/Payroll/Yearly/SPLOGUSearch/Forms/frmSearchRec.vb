
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports System.IO
Imports System.Data.SqlClient

Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Columns

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities



Public Class frmSearchRec


	Private Const strValueSeprator As String = "#@"

	Private m_Logger As ILogger = New Logger()
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsData As New ClsDivFunc

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView

#Region "private properties"
	Private Property SelectedValue As String

#End Region



#Region "public properties"
	Public Property Field2Select As String
	Public Property CurrentYear As Integer
	Public Property CurrentMonth As Integer

	Public ReadOnly Property iListeValue() As String

		Get
			Return SelectedValue

		End Get

	End Property


#End Region



#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()
		m_InitializationData = _setting

		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

	End Sub

#End Region




	Private Sub frmSearchRec_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation_Search = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.iSearch_Width = Me.Width
				My.Settings.iSearch_Height = Me.Height

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.cmdClose.Text = m_Translate.GetSafeTranslationValue(cmdClose.Text)
			Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(cmdOK.Text)


		Catch ex As Exception

		End Try

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				If My.Settings.frmLocation_Search <> String.Empty Then
					Me.Width = My.Settings.iSearch_Width
					Me.Height = My.Settings.iSearch_Height
					Dim aLoc As String() = My.Settings.frmLocation_Search.Split(CChar(";"))

					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
					End If
					Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim dt As New DataTable
			Select Case Me.Field2Select.ToUpper
				Case "MANr".ToUpper
					dt = GetMADbData4QSTSearch()

				Case Else
					Exit Sub

			End Select
			grdGrid.DataSource = dt
			grdGrid.MainView = grdGrid.CreateView("view_MAtemp")
			grdGrid.Name = "grd_ESTemp"

			grdGrid.ForceInitialize()
			grdGrid.Visible = False
			Me.PanelControl1.Controls.AddRange(New Control() {grdGrid})
			grdGrid.Dock = DockStyle.Fill
			AddHandler grdGrid.DoubleClick, AddressOf ViewKD_RowClick

			grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
			grdView.ShowFindPanel()
			grdView.OptionsView.ShowIndicator = False
			grdView.OptionsView.ShowAutoFilterRow = True
			grdView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

			grdView.OptionsSelection.MultiSelect = True	'Me.Field2Select = "MA-Nr."
			grdView.OptionsBehavior.Editable = False
			grdView.OptionsSelection.EnableAppearanceFocusedCell = False
			grdView.OptionsSelection.InvertSelection = False
			grdView.OptionsSelection.EnableAppearanceFocusedRow = True
			grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			grdView.OptionsView.ShowGroupPanel = False
			grdGrid.ForceInitialize()

			Dim i As Integer = 0
			For Each col As GridColumn In grdView.Columns
				Trace.WriteLine(String.Format("{0}", col.FieldName))
				col.MinWidth = 0
				Try
					col.Visible = True ' col.FieldName.ToLower.Contains("firma1")
					col.Caption = _ClsProgSetting.TranslateText(col.GetCaption)
					col.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains

				Catch ex As Exception
					col.Visible = False

				End Try
				i += 1
			Next col

			grdGrid.Visible = True

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Sub ViewKD_RowClick(sender As Object, e As System.EventArgs)
		Dim strValue As String = String.Empty
		Dim strTbleName As String = String.Empty
		Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView

		grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

		Try
			For i As Integer = 0 To grdView.SelectedRowsCount - 1
				Dim row As Integer = (grdView.GetSelectedRows()(i))
				If (grdView.GetSelectedRows()(i) >= 0) Then
					Dim dtr As DataRow
					dtr = grdView.GetDataRow(grdView.GetSelectedRows()(i))
					strValue &= If(strValue = "", "", strValueSeprator) & dtr.Item(Me.Field2Select).ToString

				End If
			Next i
			SelectedValue = strValue

		Catch ex As Exception

		End Try

		Me.Close()

	End Sub

	Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
		ViewKD_RowClick(Me.grdGrid, e)
	End Sub

	Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		_ClsData.GetSelektion = String.Empty
		Me.Close()
		Me.Dispose()
	End Sub

#Region "Datenbankabfragen für SearchRec..."


	Function GetMADbData4QSTSearch() As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim strQuery As String = "[List EmployeeData For Search GU SearchList]"
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("Year", m_Utility.ReplaceMissing(CurrentYear, Now.Year))
		param = cmd.Parameters.AddWithValue("Month", m_Utility.ReplaceMissing(CurrentMonth, Now.Month))

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "MAData")

		Return ds.Tables(0)
	End Function

#End Region




End Class




'Imports SP.Infrastructure.UI
'Imports SP.Infrastructure.Utility

'Imports SPProgUtility.CommonSettings
'Imports SPProgUtility.Mandanten
'Imports SPProgUtility.MainUtilities

'Imports System.IO
'Imports System.Data.SqlClient
'Imports NLog
'Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions


'Public Class frmSearchRec
'  Inherits DevExpress.XtraEditors.XtraForm
'  Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _ClsData As New ClsDivFunc

'  Dim bAllowedtowrite As Boolean

'	Private m_md As Mandant
'	Private m_Utility As SPProgUtility.MainUtilities.Utilities
'	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

'	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
'	''' <summary>
'	''' The translation value helper.
'	''' </summary>
'	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

'  Public ReadOnly Property iLOAGValue(ByVal strValue As String) As String

'    Get
'      Dim strBez As String = String.Empty

'      If Me.LvData.SelectedItems.Count > 0 Then strBez = _ClsData.GetSelektion
'      ClsDataDetail.strLOAGData = strBez

'      Return ClsDataDetail.strLOAGData
'    End Get

'  End Property

'  Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
'    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'		Dim Time_1 As Double = System.Environment.TickCount

'		Try
'      Dim strStyleQuery As String = "//Layouts/Form_DevEx/FormStyle"
'      Dim strStyleName As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strStyleQuery, String.Empty)
'      If strStyleName <> String.Empty Then
'        Me.LookAndFeel.UseDefaultLookAndFeel = False
'        Me.LookAndFeel.UseWindowsXPTheme = False
'        Me.LookAndFeel.SkinName = strStyleName
'      End If

'    Catch ex As Exception
'      logger.Error(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))

'    End Try

'    Try
'      ' Die DropDownListBox für die Sortierung füllen
'      Select Case ClsDataDetail.StrButtonValue
'        Case ClsDataDetail.ButtonValue.Arbeitgeberlohnart
'          Me.cboDbField.Items.Add("")
'          If ClsDataDetail.Get4What = ClsDataDetail.What.MANR Then
'            Me.cboDbField.Items.Add("MANR")
'						Me.cboDbField.Items.Add(m_Translate.GetSafeTranslationValue("Nachnamen"))
'						Me.cboDbField.Items.Add(m_Translate.GetSafeTranslationValue("Vornamen"))
'					End If
'					Me.cboDbField.SelectedIndex = 0
'				Case Else
'					Me.cboDbField.Visible = False
'					Me.lbSuchfeld.Visible = False
'			End Select

'			SetLvwHeader()

'			FillLvData(CShort(Me.cboDbField.SelectedIndex), Me.txtSearchValue.Text)
'			bAllowedtowrite = True

'			' Set Focus to Textbox
'			Me.txtSearchValue.Focus()

'		Catch ex As Exception	' Manager
'			m_UtilityUi.ShowErrorDialog(ex.ToString)
'		End Try


'	End Sub

'	Sub SetLvwHeader()
'		Try
'			Dim strColumnString As String = String.Empty
'			Dim strColumnWidthInfo As String = String.Empty
'			Dim strUSLang As String = String.Empty ' _ClsProgSetting.GetUSLanguage()

'			Select Case ClsDataDetail.StrButtonValue
'				Case ClsDataDetail.ButtonValue.Arbeitgeberlohnart
'					strColumnString = "Res;MANr;Kandidatname;Vorname;PLZ;Ortschaft"
'					' Default
'					strColumnWidthInfo = "0-0;50-0;200-0;100-0;50-2;120-0"
'					If My.Settings.LV_2_1_Size <> String.Empty Then strColumnWidthInfo = My.Settings.LV_2_1_Size

'				Case Else
'					strColumnString = "Res;Bezeichnung"
'					strColumnWidthInfo = "0-0;500-0"
'					If My.Settings.LV_2_2_Size <> String.Empty Then strColumnWidthInfo = My.Settings.LV_2_2_Size

'			End Select
'			strColumnString = m_Translate.GetSafeTranslationValue(strColumnString)
'      FillDataHeaderLv(Me.LvData, strColumnString, strColumnWidthInfo)

'    Catch ex As Exception ' Manager
'			m_UtilityUi.ShowErrorDialog(ex.ToString)
'		End Try

'  End Sub

'  Sub FillLvData(ByVal strField As Short, ByVal strFieldValue As String)
'    Dim strOperator As String = "="
'    Dim strSqlQuery As String = ""
'    Dim i As Integer = 0

'    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
'    Dim cmd As New System.Data.SqlClient.SqlCommand("", Conn)
'    cmd.CommandType = CommandType.StoredProcedure

'    Try

'      ' Auswahl-Button deaktivieren
'      Me.cmdOK.Enabled = False
'      Conn.Open()

'      Dim sortKey As Integer = 0
'      strFieldValue = strFieldValue.Replace("*", "%")

'      ' SortKey. Wenn was angegeben und keine Zahl ist.
'      If strFieldValue.Length > 0 And Not IsNumeric(strFieldValue) Then
'        sortKey = 1
'        strFieldValue = String.Format("{0}%", strFieldValue)
'      Else
'        ' SortKey. Wenn nichts oder Nummer angegeben.

'        Select Case ClsDataDetail.Get4What
'          Case ClsDataDetail.What.MANR
'            sortKey = 0
'            If strFieldValue.Length = 0 Then
'              strFieldValue = "0"
'            End If
'        End Select
'      End If


'      ' Nach was soll explizit sortiert werden.
'      Select Case strField
'        Case 0
'          If strFieldValue.Replace("%", "").Length = 0 Then
'            strFieldValue = "0"
'          End If
'        Case 1
'          strFieldValue = String.Format("{0}%", strFieldValue)
'        Case 2
'          strFieldValue = String.Format("{0}%", strFieldValue)
'      End Select
'      sortKey = strField


'      ' Zu verwendende gespeicherte Prozedur bestimmen und Parameter hinzufügen.
'      Select Case ClsDataDetail.Get4What
'        Case ClsDataDetail.What.MANR
'          strSqlQuery = "[List Data For MASearch]"
'          cmd.Parameters.AddWithValue("@SortKey", sortKey)
'          cmd.Parameters.AddWithValue("@Parameter", strFieldValue)
'      End Select

'      cmd.CommandText = strSqlQuery

'      Dim rBLJrec As SqlDataReader = cmd.ExecuteReader
'      Me.LvData.Items.Clear()
'      Me.LvData.FullRowSelect = True
'      Dim subItem As ListViewItem.ListViewSubItem

'      While rBLJrec.Read

'        Select Case ClsDataDetail.Get4What
'          Case ClsDataDetail.What.MANR
'            ' Reserve
'            Me.LvData.Items.Add("")

'            ' MANr
'            If Not IsDBNull(rBLJrec("MANr")) Then
'              subItem = New ListViewItem.ListViewSubItem
'              subItem.Name = "MANr"
'              subItem.Text = rBLJrec("MANr").ToString
'              Me.LvData.Items(i).SubItems.Add(subItem)
'            End If

'            ' Nachname
'            If Not IsDBNull(rBLJrec("Nachname")) Then
'              subItem = New ListViewItem.ListViewSubItem
'              subItem.Name = "Nachname"
'              subItem.Text = rBLJrec("Nachname").ToString
'              Me.LvData.Items(i).SubItems.Add(subItem)
'            End If

'            ' Vorname
'            If Not IsDBNull(rBLJrec("Vorname")) Then
'              subItem = New ListViewItem.ListViewSubItem
'              subItem.Name = "Vorname"
'              subItem.Text = rBLJrec("Vorname").ToString
'              Me.LvData.Items(i).SubItems.Add(subItem)
'            End If

'            ' PLZ
'            If Not IsDBNull(rBLJrec("PLZ")) Then
'              subItem = New ListViewItem.ListViewSubItem
'              subItem.Name = "PLZ"
'              subItem.Text = rBLJrec("PLZ").ToString
'              Me.LvData.Items(i).SubItems.Add(subItem)
'            End If

'            ' Ort
'            If Not IsDBNull(rBLJrec("Ort")) Then
'              subItem = New ListViewItem.ListViewSubItem
'              subItem.Name = "Ort"
'              subItem.Text = rBLJrec("Ort").ToString
'              Me.LvData.Items(i).SubItems.Add(subItem)
'            End If

'          Case Else
'            ' Reserve
'            Me.LvData.Items.Add("")

'            ' Bezeichnung
'            If Not IsDBNull(rBLJrec("Bezeichnung")) Then
'              subItem = New ListViewItem.ListViewSubItem
'              subItem.Name = "Bezeichnung"
'              subItem.Text = rBLJrec("Bezeichnung").ToString
'              Me.LvData.Items(i).SubItems.Add(subItem)
'            Else
'              Me.LvData.Items(i).SubItems.Add("")
'            End If

'        End Select

'        i += 1

'      End While


'    Catch ex As Exception ' Manager
'      Me.LvData.Items.Clear()
'      m_UtilityUi.ShowErrorDialog("FillLvData", ex)

'    Finally
'      Conn.Close()
'      Conn.Dispose()

'    End Try

'  End Sub

'  Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
'    Me.Close()
'  End Sub

'  Private Sub LvData_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles LvData.ColumnWidthChanged
'    If bAllowedtowrite Then SaveLV_ColumnInfo()
'  End Sub

'  Private Sub LvData_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles LvData.DoubleClick
'    cmdOK_Click(sender, e)
'  End Sub

'  Private Sub LvData_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LvData.MouseClick
'    Try
'      Dim strValue As String = String.Empty
'      Dim strBez As String = String.Empty

'      If ClsDataDetail.StrButtonValue = ClsDataDetail.ButtonValue.Arbeitgeberlohnart Then
'        For Each index As Integer In LvData.SelectedIndices
'          strBez += LvData.Items(index).SubItems(ClsDataDetail.Get4What.ToString).Text & "#@"
'        Next
'      Else
'        For Each index As Integer In LvData.SelectedIndices
'          strBez += LvData.Items(index).SubItems("Bezeichnung").Text & "#@"
'        Next
'      End If

'      If strBez.EndsWith("#@") Then strBez = Mid(strBez, 1, Len(strBez) - 2)
'      _ClsData.GetSelektion = strBez

'      Me.cmdOK.Enabled = LvData.SelectedIndices.Count > 0

'    Catch ex As Exception ' Manager
'			m_UtilityUi.ShowErrorDialog(ex.ToString)
'		End Try

'  End Sub

'  Private Sub txtSearchValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearchValue.KeyPress, cmdSearch.KeyPress, cboDbField.KeyPress, cmdOK.KeyPress, cmdClose.KeyPress

'    Try
'      If Asc(e.KeyChar) = Keys.Enter Then
'        FillLvData(CShort(Me.cboDbField.SelectedIndex), Me.txtSearchValue.Text)
'      ElseIf Asc(e.KeyChar) = Keys.Escape Then
'        cmdClose_Click(sender, New System.EventArgs)
'      End If
'    Catch ex As Exception ' Manager
'			m_UtilityUi.ShowErrorDialog(ex.ToString)
'		End Try


'  End Sub

'  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
'    _ClsData.GetSelektion = String.Empty
'    Me.Close()
'    Me.Dispose()
'  End Sub

'  Private Sub LvData_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles LvData.KeyPress
'    Try
'      If e.KeyChar = Chr(13) Then ' Enter
'        LvData_MouseClick(sender, New MouseEventArgs(Windows.Forms.MouseButtons.Left, 0, 0, 0, 0))
'        cmdOK_Click(sender, New System.EventArgs)
'      ElseIf Asc(e.KeyChar) = Keys.Escape Then
'        cmdClose_Click(sender, New System.EventArgs)
'      End If
'    Catch ex As Exception ' Manager
'			m_UtilityUi.ShowErrorDialog(ex.ToString)
'		End Try

'  End Sub

'  Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
'    Try
'      FillLvData(CShort(Me.cboDbField.SelectedIndex), Me.txtSearchValue.Text)
'    Catch ex As Exception ' Manager
'			m_UtilityUi.ShowErrorDialog(ex.ToString)
'		End Try

'  End Sub

'  Private Sub LvData_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LvData.KeyUp
'    Try
'      LvData_MouseClick(sender, New MouseEventArgs(Windows.Forms.MouseButtons.Left, 0, 0, 0, 0))
'    Catch ex As Exception ' Manager
'			m_UtilityUi.ShowErrorDialog(ex.ToString)
'		End Try

'  End Sub

'  Sub SaveLV_ColumnInfo()
'    Try
'      Dim strColInfo As String = String.Empty
'      Dim strColInfo_1 As String = String.Empty
'      Dim strColAlign As String = String.Empty

'      For i As Integer = 0 To LvData.Columns.Count - 1
'        If LvData.Columns.Item(i).TextAlign = HorizontalAlignment.Center Then
'          strColAlign = "2"

'        ElseIf LvData.Columns.Item(i).TextAlign = HorizontalAlignment.Right Then
'          strColAlign = "1"
'        Else
'          strColAlign = "0"
'        End If

'        strColInfo &= CStr(IIf(strColInfo = String.Empty, "", ";")) & (LvData.Columns.Item(i).Width) & "-" & strColAlign

'      Next

'      Try
'        If ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.Arbeitgeberlohnarten Then
'          My.Settings.LV_2_1_Size = strColInfo
'        ElseIf ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.Lohnartenrekapitulation Then
'          My.Settings.LV_2_2_Size = strColInfo
'        End If

'        Trace.WriteLine(strColInfo)
'        My.Settings.Save()

'      Catch ex As Exception
'        ' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
'      End Try
'    Catch ex As Exception ' Manager
'			m_UtilityUi.ShowErrorDialog(ex.ToString)
'    End Try

'  End Sub

'End Class