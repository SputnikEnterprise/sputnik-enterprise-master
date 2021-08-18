
Imports System.IO
Imports System.Data.SqlClient
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.LookAndFeel

Public Class frmSearchRec
  Inherits DevExpress.XtraEditors.XtraForm

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsData As New ClsDivFunc

  Private Property GetMDYear As Integer
  Private Property Field2Select As String

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView

  Public ReadOnly Property iLoNLAValue(ByVal strValue As String) As String

    Get
      Dim strBez As String = String.Empty

      'If Me.LvData.SelectedItems.Count > 0 Then
      strBez = _ClsData.GetSelektion
      'End If

      ClsDataDetail.strLoNLAData = strBez

      Return ClsDataDetail.strLoNLAData
    End Get

  End Property

  Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    Try
      Dim strQuery As String = "//Layouts/Form_DevEx/FormStyle"
      Dim strStyleName As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, String.Empty)
      If strStyleName <> String.Empty Then
        UserLookAndFeel.Default.SetSkinStyle(strStyleName)
      End If

    Catch ex As Exception

    End Try

		Try
			Dim dt As New DataTable
			dt = GetMADbData4NLA(Me.GetMDYear)
			grdGrid.DataSource = dt
			grdGrid.MainView = grdGrid.CreateView("view_MAtemp")
			grdGrid.Name = "grd_MATemp"

			grdGrid.ForceInitialize()
			grdGrid.Visible = False
			'Me.Controls.AddRange(New Control() {pcc})
			Me.PanelControl1.Controls.AddRange(New Control() {grdGrid})
			'If My.Settings.pcc_EnterpriseSize <> "" Then
			'  Dim aSize As String() = My.Settings.pcc_EnterpriseSize.Split(CChar(";"))
			'  pcc.Size = New Size(CInt(aSize(0)), CInt(aSize(1)))
			'Else
			'pcc.Size = New Size(600, 400)
			grdGrid.Dock = DockStyle.Fill
			'End If

			'    AddHandler GridView1.RowClick, AddressOf EditValueChanged
			'AddHandler pcc.SizeChanged, AddressOf pcc_SizeChanged
			AddHandler grdGrid.DoubleClick, AddressOf ViewKD_RowClick

			'      pcc.ShowPopup(Cursor.Position)
			grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
			'If My.Settings.bgrdView_EnterpriseShowGroup Then _
			'  grdView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways
			grdView.ShowFindPanel()

			grdView.OptionsSelection.MultiSelect = Me.Field2Select = "MANr"
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

				Catch ex As Exception
					col.Visible = False

				End Try
				i += 1
			Next col
			If Me.Field2Select <> "MANr" Then
				grdView.Columns("MANr").Visible = False
			End If


			grdGrid.Visible = True
			'pcc.ResumeLayout()


		Catch ex As Exception

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
					strValue &= If(strValue = "", "", ", ") & dtr.Item(Me.Field2Select).ToString

				End If
			Next i

			_ClsData.GetSelektion = strValue

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

	Public Sub New()

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		GetMDYear = Year(Now)

	End Sub

  Public Sub New(ByVal _field2Select As String, ByVal iYear As Integer)

    ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Me.GetMDYear = iYear
    Me.Field2Select = _field2Select
    ClsDataDetail.strLoNLAData = String.Empty

  End Sub

End Class