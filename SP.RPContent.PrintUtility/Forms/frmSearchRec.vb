

Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions
Imports SP.Infrastructure.Logging
Imports DevExpress.LookAndFeel

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.ComponentModel


Public Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML
  Private m_md As Mandant
  Private m_utility As Utilities

  Public Property RecCount As Integer
  Private Property Sql2Open As String


  Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Private _ClsReg As New SPProgUtility.ClsDivReg
  Private _ClsData As New ClsDivFunc
  Private bAllowedtowrite As Boolean

  Private _frmSetting As New ClsPopupSetting

  Private Property GetMDYear As New List(Of Integer)
  Private Property GetMDMonth As New List(Of Short)
  Private Property GetGAVBez As String
  Private Property GetSearchField As String

  Private grdGrid As New DevExpress.XtraGrid.GridControl
  Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView



#Region "Constuctor"

  Public Sub New(ByVal _setting As ClsPopupSetting)

    ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.Skins.SkinManager.EnableFormSkins()

    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    ResetGridData()

    Me._frmSetting = _setting

    Me.GetGAVBez = _setting.SearchPVLBez
    Me.GetMDYear = _setting.SearchYear
    Me.GetMDMonth = _setting.SearchMonth
    Me.GetSearchField = _setting.SearchedField

    ClsDataDetail.strButtonValue = String.Empty

  End Sub


#End Region



  ''' <summary>
  ''' Gets the selected employee.
  ''' </summary>
  ''' <returns>The selected employee or nothing if none is selected.</returns>
  Public ReadOnly Property SelectedRecord As FoundedRPData
    Get
      Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

      If Not (gvRP Is Nothing) Then

        Dim selectedRows = gvRP.GetSelectedRows()

        If (selectedRows.Count > 0) Then
          Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedRPData)
          Return employee
        End If

      End If

      Return Nothing
    End Get

  End Property

  Function GetDbData4Show() As IEnumerable(Of FoundedRPData)
    Dim result As List(Of FoundedRPData) = Nothing
    m_utility = New Utilities
    Dim strMonth As String = String.Empty
    Dim strYear As String = String.Empty

    Dim sql As String

    sql = "Select RP.RPNr, (Convert(nvarchar(10), RP.Von, 104) + ' - '  + Convert(nvarchar(10), RP.Bis, 104)) As Zeitraum, "
    sql &= "(MA.Nachname + ', ' + MA.Vorname) As [Kandidat], "
    sql &= "KD.Firma1, ES.ES_Als, RP.RPGAV_Beruf From RP "
    sql &= "Left Join Mitarbeiter MA On RP.MANr = MA.MANr "
    sql &= "Left Join Kunden KD On RP.KDNr = KD.KDNr "
    sql &= "Left Join ES On RP.ESNr = ES.ESNr "
    sql &= "Where RP.Jahr In (@iYear) And "
    sql &= "RP.Monat In (@iMonth) And "
    sql &= "(RP.RPGAV_Beruf = @GAVBez Or @GAVBez = '') And "
    sql &= "RP.MDNr = @MDNr And "
    sql &= "(MA.MAFiliale + KD.KDFiliale) Like "
    sql &= "@USFiliale "
    sql &= "Order By MA.Nachname"

    For i As Integer = 0 To Me._frmSetting.SearchMonth.Count - 1
      strMonth &= If(strMonth = "", "", ",") & Me._frmSetting.SearchMonth(i)
    Next
    For i As Integer = 0 To Me._frmSetting.SearchYear.Count - 1
      stryear &= If(stryear = "", "", ",") & Me._frmSetting.SearchYear(i)
    Next

    Dim listOfParams As New List(Of SqlClient.SqlParameter)
    listOfParams.Add(New SqlClient.SqlParameter("@GAVBez", _frmSetting.SearchPVLBez))
    listOfParams.Add(New SqlClient.SqlParameter("@iMonth", strMonth))
    listOfParams.Add(New SqlClient.SqlParameter("@iYear", strYear))
    listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ClsDataDetail.ProgSettingData.SelectedMDNr))
    listOfParams.Add(New SqlClient.SqlParameter("@USFiliale", "%%"))

    Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, listOfParams)

    Try

      If (Not reader Is Nothing) Then

        result = New List(Of FoundedRPData)

        While reader.Read()
          Dim overviewData As New FoundedRPData

          overviewData.RPNr = CInt(m_utility.SafeGetInteger(reader, "RPNr", 0))
          overviewData.rpperiode = m_utility.SafeGetString(reader, "zeitraum")

          overviewData.employeename = String.Format("{0}", m_utility.SafeGetString(reader, "Kandidat"))
          overviewData.customername = String.Format("{0}", m_utility.SafeGetString(reader, "Firma1"))
          overviewData.gavberuf = String.Format("{0}", m_utility.SafeGetString(reader, "RPGAV_Beruf"))

          overviewData.es_als = String.Format("{0}", m_utility.SafeGetString(reader, "es_als"))

          result.Add(overviewData)

        End While

      End If

    Catch e As Exception
      result = Nothing
      m_Logger.LogError(e.ToString())

    Finally
      m_utility.CloseReader(reader)

    End Try

    Return result
  End Function

  Private Function LoadFoundedRPList() As Boolean

    Dim listOfEmployees = GetDbData4Show()

    Dim myGridData = (From person In listOfEmployees
    Select New FoundedRPData With
           {.RPNr = person.RPNr,
            .rpperiode = person.rpperiode,
            .employeename = person.employeename,
            .customername = person.customername,
          .es_als = person.es_als,
          .gavberuf = person.gavberuf}).ToList()

    Dim listDataSource As BindingList(Of FoundedRPData) = New BindingList(Of FoundedRPData)

    For Each p In myGridData
      listDataSource.Add(p)
    Next

    grdRP.DataSource = listDataSource

    Return Not listOfEmployees Is Nothing
  End Function


  Private Sub ResetGridData()

    gvRP.OptionsView.ShowIndicator = False
    gvRP.OptionsView.ShowAutoFilterRow = True
    gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
    gvRP.OptionsSelection.MultiSelect = True
    gvRP.Columns.Clear()

    Dim columnRPNumber As New DevExpress.XtraGrid.Columns.GridColumn()
    columnRPNumber.Caption = "Rapport-Nr."
    columnRPNumber.Name = "RPNr"
    columnRPNumber.FieldName = "RPNr"
    columnRPNumber.Visible = True
    gvRP.Columns.Add(columnRPNumber)

    Dim columnRPPeriode As New DevExpress.XtraGrid.Columns.GridColumn()
    columnRPPeriode.Caption = "Zeitraum"
    columnRPPeriode.Name = "rpperiode"
    columnRPPeriode.FieldName = "rpperiode"
    columnRPPeriode.Visible = True
    gvRP.Columns.Add(columnRPPeriode)

    Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
    columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    columnEmployee.Caption = "Kandidat"
    columnEmployee.Name = "employeename"
    columnEmployee.FieldName = "employeename"
    columnEmployee.Visible = True
    gvRP.Columns.Add(columnEmployee)

    Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
    columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    columnCustomer.Caption = "Kunde"
    columnCustomer.Name = "cusotmername"
    columnCustomer.FieldName = "customername"
    columnCustomer.Visible = True
    gvRP.Columns.Add(columnCustomer)


    Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
    columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    columnESAls.Caption = "ES-Als"
    columnESAls.Name = "es_als"
    columnESAls.FieldName = "es_als"
    columnESAls.Visible = True
    gvRP.Columns.Add(columnESAls)

    Dim columngavberuf As New DevExpress.XtraGrid.Columns.GridColumn()
    columngavberuf.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    columngavberuf.Caption = "GAV-Beruf"
    columngavberuf.Name = "gavberuf"
    columngavberuf.FieldName = "gavberuf"
    columngavberuf.Visible = True
    gvRP.Columns.Add(columngavberuf)

    grdRP.DataSource = Nothing

  End Sub


  Public ReadOnly Property iMyValue(ByVal strValue As String) As String

    Get
      Dim strBez As String = String.Empty

      'If Me.LvData.SelectedItems.Count > 0 Then
      strBez = _ClsData.GetSelektion
      'End If

      ClsDataDetail.strButtonValue = strBez

      Return ClsDataDetail.strButtonValue
    End Get

  End Property

  Private Sub frmOnDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        My.Settings.frm_LVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
        My.Settings.ifrmLVHeight = Me.Height
        My.Settings.ifrmLVWidth = Me.Width

        My.Settings.Save()
      End If


    Catch ex As Exception
      ' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
    End Try

  End Sub

  Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    m_md = New Mandant

    Try
      Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
      If strStyleName <> String.Empty Then
        UserLookAndFeel.Default.SetSkinStyle(strStyleName)
      End If

    Catch ex As Exception

    End Try

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
      Dim m_xml As New ClsXML
      Dim Time_1 As Double = System.Environment.TickCount
      m_xml.GetChildChildBez(Me)
      Dim Time_2 As Double = System.Environment.TickCount
      Trace.WriteLine("1. Verbrauchte Zeit: " & ((Time_2 - Time_1) / 1000) & " s.")

    Catch ex As Exception ' Manager
      MessageBoxShowError("frmDataSel_Load", ex)
    End Try

    Try
      LoadFoundedRPList()

    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

    End Try

  End Sub


  Public ReadOnly Property GetSelectedValues() As String

    Get

      Try
        Dim strValue As String = String.Empty

        For i As Integer = 0 To gvRP.SelectedRowsCount - 1
          Dim row As Integer = (gvRP.GetSelectedRows()(i))
          If (gvRP.GetSelectedRows()(i) >= 0) Then

            Dim selectedRows = gvRP.GetSelectedRows()

            If (selectedRows.Count > 0) Then
              Dim employee = CType(gvRP.GetRow(selectedRows(i)), FoundedRPData)
              strValue &= If(strValue = "", "", ", ") & employee.RPNr
            End If

          End If

        Next i

        Return strValue

      Catch ex As Exception
        Return Nothing
      End Try

    End Get

  End Property

  Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
    Me.Close()
  End Sub

  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
    _ClsData.GetSelektion = String.Empty
    Me.Close()
    Me.Dispose()
  End Sub

End Class