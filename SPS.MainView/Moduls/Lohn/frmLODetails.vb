
Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SP.Infrastructure.Logging

Public Class frmLODetails
  Private Shared m_Logger As ILogger = New Logger()


  Private _ClsSetting As New ClsLOSetting
  Private Property Modul2Open As String

  Private Property MetroForeColor As System.Drawing.Color
  Private Property MetroBorderColor As System.Drawing.Color

  Public Sub New(ByVal _setting As ClsLOSetting, ByVal _m2Open As String)

    ' Dieser Aufruf ist für den Designer erforderlich.
    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.Skins.SkinManager.EnableFormSkins()

    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Me._ClsSetting = _setting
    Me.Modul2Open = _m2Open
    Me._ClsSetting.OpenDetailModul = _m2Open

    RemoveHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged
    Me.chkSelMA.Checked = Me._ClsSetting.Data4SelectedMA
    AddHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged

  End Sub

  Private Sub frmMADetails_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        My.Settings.SETTING_CANDIDAT_LOCATION = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
        My.Settings.SETTING_CANDIDAT_WIDTH = Me.Width
        My.Settings.SETTING_CANDIDAT_HEIGHT = Me.Height

        My.Settings.Save()
      End If

    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

    End Try

  End Sub

  Private Sub frmDetails_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strTitle As String = String.Empty

		Try
			If My.Settings.SETTING_CANDIDAT_HEIGHT > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.SETTING_CANDIDAT_HEIGHT)
			If My.Settings.SETTING_CANDIDAT_WIDTH > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.SETTING_CANDIDAT_WIDTH)
			If My.Settings.SETTING_CANDIDAT_LOCATION <> String.Empty Then
				Dim aLoc As String() = My.Settings.SETTING_CANDIDAT_LOCATION.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formsizing. {1}", strMethodeName, ex.Message))

      End Try

			Me.sccMain.Dock = DockStyle.Fill
			Me.bsiRecCount.Caption = TranslateText("Bereit")

    Me.gvDetail.OptionsView.ShowIndicator = False
    Select Case Me.Modul2Open.ToLower
      Case "LOES".ToLower
        strTitle = "Anzeige der Einsätze"
        Dim _ClsES As New ClsLOESDetails(Me._ClsSetting)
        _ClsES.FillMAOpenES(Me.grdDetailrec)

      Case "LORP".ToLower
        strTitle = "Anzeige der Rapporte"
        Me._ClsSetting.gvDetailDisplayMember = "Rapport-Nr."
        Dim _ClsRP As New ClsloRPDetails(Me._ClsSetting)
        _ClsRP.FillMAOpenRP(Me.grdDetailrec)

      Case "LOZG".ToLower
        strTitle = "Anzeige der Auszahlungen"
        Me._ClsSetting.gvDetailDisplayMember = "Auszahlung-Nr."
        Dim _ClsZG As New ClsLOZGDetails(Me._ClsSetting)
        _ClsZG.FillMAOpenZG(Me.grdDetailrec)


      Case Else

    End Select
    Me.Text = String.Format(TranslateText(strTitle))
		Me.rlblDetailHeader.Text = String.Format("<b>{0}</b>", TranslateText(strTitle))
		Me.bsiRecCount.Caption = String.Format(TranslateText("Anzahl Datensätze: {0}"), gvDetail.RowCount)

  End Sub

  Private Sub chkSelMA_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles chkSelMA.CheckedChanged

    grdDetailrec.BeginUpdate()
    Me.gvDetail.Columns.Clear()
    Me.grdDetailrec.DataSource = Nothing

    Me._ClsSetting.Data4SelectedMA = Me.chkSelMA.Checked
    Select Case Me.Modul2Open.ToLower
      Case "LOES".ToLower
        Me._ClsSetting.gvDetailDisplayMember = "Einsatz-Nr."
        Dim _ClsES As New ClsLOESDetails(Me._ClsSetting)
        _ClsES.FillMAOpenES(Me.grdDetailrec)

      Case "LORP".ToLower
        Me._ClsSetting.gvDetailDisplayMember = "Rapport-Nr."
        Dim _ClsRP As New ClsloRPDetails(Me._ClsSetting)
        _ClsRP.FillMAOpenRP(Me.grdDetailrec)

      Case "LOZG".ToLower
        Me._ClsSetting.gvDetailDisplayMember = "Auszahlung-Nr."
        Dim _ClsZG As New ClsLOZGDetails(Me._ClsSetting)
        _ClsZG.FillMAOpenZG(Me.grdDetailrec)

      Case Else

    End Select
    Me.grdDetailrec.EndUpdate()
    Me.bsiRecCount.Caption = String.Format(TranslateText("Anzahl Datensätze: {0}"), gvDetail.RowCount)

  End Sub

  Private Sub gvDetail_DoubleClick(sender As Object, e As System.EventArgs) Handles gvDetail.DoubleClick

    Select Case Me.Modul2Open.ToLower
      Case "loES".ToLower
        Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedESNr = Me._ClsSetting.SelectedDetailNr})
				_ClsES.OpenSelectedES(Me._ClsSetting.SelectedMDNr, ModulConstants.UserData.UserNr)

      Case "loRp".ToLower
        Dim _ClsRP As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedmdNr,
                                                            .SelectedRPNr = Me._ClsSetting.SelectedDetailNr})
				_ClsRP.OpenSelectedReport(_ClsSetting.SelectedMDNr, ModulConstants.UserData.UserNr)

      Case "loZG".ToLower
        Dim _ClsZG As New ClsOpenModul(New ClsSetting With {.SelectedZGNr = Me._ClsSetting.SelectedDetailNr})
				_ClsZG.OpenSelectedAdvancePayment(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
				'_ClsZG.OpenSelectedZG()

      Case "lolm".ToLower
				Dim _ClsRP As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																														.SelectedProposeNr = Me._ClsSetting.SelectedDetailNr})
				_ClsRP.OpenSelectedProposeTiny(_ClsSetting.SelectedMDNr, ModulConstants.UserData.UserNr)

			Case Else

    End Select

  End Sub

  Private Sub gvDetail_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvDetail.FocusedRowChanged
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strValue As Integer = 0
    Dim view As GridView = TryCast(sender, GridView)

    Try
      For i As Integer = 0 To view.SelectedRowsCount - 1
        Dim row As Integer = (view.GetSelectedRows()(i))

        If (view.GetSelectedRows()(i) >= 0) Then
          Dim dtr As DataRow
          dtr = view.GetDataRow(gvDetail.GetSelectedRows()(i))
          strValue = CInt(dtr.Item(Me._ClsSetting.gvDetailDisplayMember).ToString)

        Else
          Exit Sub

        End If
      Next i
      Me._ClsSetting.SelectedDetailNr = strValue


    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.Navigationsleiste. {1}", strMethodeName, ex.Message))
      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

    End Try

  End Sub


End Class