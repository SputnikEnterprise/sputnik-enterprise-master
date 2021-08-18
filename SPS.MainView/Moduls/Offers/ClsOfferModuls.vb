
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient

Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid
Imports System.Threading

Public Class ClsOfferModuls
  Private Shared m_Logger As ILogger = New Logger()

  Private _ClsSetting As New ClsOfferSetting
  Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private _PrintjobSetting As New ClsPrintSetting
	Private mgr As New DevExpress.XtraBars.BarManager
	Private itm As New DevExpress.XtraBars.BarButtonItem

  Public Sub New(ByVal _setting As ClsOfferSetting)
    Me._ClsSetting = _setting
  End Sub

#Region "Funktionen für Kontextmenü..."

  ''' <summary>
  ''' Creates contextmenu for printbutton in Massenversand
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <remarks></remarks>
  Sub ShowContextMenu4Print(sender As DevExpress.XtraEditors.DropDownButton)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
    Dim strQuery As String = "[Get ContexMenuItems 4 Print In MainView]"
    Dim popupMenu1 As New DevExpress.XtraBars.PopupMenu
    'Dim mgr As New DevExpress.XtraBars.BarManager

    mgr = New DevExpress.XtraBars.BarManager
    popupMenu1.Manager = mgr
    sender.DropDownControl = popupMenu1

    Try
      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      Dim param As System.Data.SqlClient.SqlParameter
      cmd.CommandType = CommandType.StoredProcedure
      param = cmd.Parameters.AddWithValue("@ModulName", "OfferPrint")
			param = cmd.Parameters.AddWithValue("@lang", ModulConstants.UserData.UserLanguage)

      Dim rFrec As SqlDataReader = cmd.ExecuteReader
      _PrintjobSetting = New ClsPrintSetting

      Try
        While rFrec.Read
          itm = New DevExpress.XtraBars.BarButtonItem

          Dim strmnuBez As String = String.Empty
          Dim strmnuName As String = String.Empty
          Dim strmnuTooltip As String = String.Empty

					_PrintjobSetting.PrintJobCaption = String.Format("{0}", rFrec("TranslatedValue"))
					_PrintjobSetting.PrintJobName = String.Format("{0}", rFrec("mnuName"))
          _PrintjobSetting.PrintJobNr = String.Format("{0}", rFrec("PrintJobNr"))

					strmnuBez = String.Format("{0}", rFrec("TranslatedValue"))
					strmnuName = String.Format("{0}", rFrec("mnuName"))

          itm.Name = strmnuName
          itm.Caption = strmnuBez

          If strmnuBez.StartsWith("_") Then
            itm.Caption = strmnuBez.Remove(0, 1)
            popupMenu1.AddItem(itm).BeginGroup = True
          Else
            popupMenu1.AddItem(itm)
          End If
          AddHandler itm.ItemClick, AddressOf GetMnuItem4Print

        End While

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.Menüs aufbauen: {1}", strMethodeName, ex.Message))
        ShowErrDetail(String.Format("{0}.Menüs aufbauen: {1}", strMethodeName, ex.Message))

      End Try


    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

    End Try

  End Sub

  ''' <summary>
  ''' wertet die contextmenu vom printbutton
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Sub GetMnuItem4Print(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
    Dim strMnuName As String = e.Item.Name.ToLower

    Select Case _PrintjobSetting.PrintJobName.ToLower
      Case "OfferStammblatt".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedOfferNr = Me._ClsSetting.SelectedOfferNr, .SelectedKDNr = _ClsSetting.SelectedKDNr, .SelectedZHDNr = _ClsSetting.SelectedZHDNr})
				obj.Printofferblatt()

			Case "Mailing".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedOfferNr = Me._ClsSetting.SelectedOfferNr, .SelectedKDNr = Me._ClsSetting.SelectedKDNr, .SelectedZHDNr = Me._ClsSetting.SelectedZHDNr})
				obj.SendOfferWithMail()

			Case Else
				Exit Sub

    End Select

  End Sub

  ''' <summary>
  ''' Creates contextmenu for newbutton in Massenversand
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <remarks></remarks>
  Sub ShowContextMenu4New(sender As DevExpress.XtraEditors.DropDownButton)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
    Dim strQuery As String = "[Get ContexMenuItems 4 NewRec In MainView]"
    Dim popupMenu1 As New DevExpress.XtraBars.PopupMenu
    Dim mgr As New DevExpress.XtraBars.BarManager

    popupMenu1.Manager = mgr
    sender.DropDownControl = popupMenu1

    Try
      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      Dim param As System.Data.SqlClient.SqlParameter
      cmd.CommandType = CommandType.StoredProcedure
      param = cmd.Parameters.AddWithValue("@ModulName", "OfferNew")
			param = cmd.Parameters.AddWithValue("@lang", ModulConstants.UserData.UserLanguage)

      Dim rFrec As SqlDataReader = cmd.ExecuteReader

      Try
        While rFrec.Read
          Dim strmnuBez As String = String.Empty
          Dim strmnuName As String = String.Empty
          Dim strmnuTooltip As String = String.Empty

					strmnuBez = String.Format("{0}", rFrec("TranslatedValue"))
					strmnuName = String.Format("{0}", rFrec("mnuName"))

          Dim itm As New DevExpress.XtraBars.BarButtonItem
          itm = New DevExpress.XtraBars.BarButtonItem

          itm.Name = strmnuName
          itm.Caption = strmnuBez

          If strmnuBez.StartsWith("_") Then
            itm.Caption = strmnuBez.Remove(0, 1)
            popupMenu1.AddItem(itm).BeginGroup = True
          Else
            popupMenu1.AddItem(itm)
          End If
          AddHandler itm.ItemClick, AddressOf GetMnuItem4New

        End While

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.Menüs aufbauen: {1}", strMethodeName, ex.Message))
        ShowErrDetail(String.Format("{0}.Menüs aufbauen: {1}", strMethodeName, ex.Message))

      End Try


    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.Datenbank öffnen. {1}", strMethodeName, ex.Message))
      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

    End Try

  End Sub

  ''' <summary>
  ''' wertet die contextmenu vom newbutton
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Sub GetMnuItem4New(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
    Dim strMnuName As String = e.Item.Name.ToLower

    Select Case strMnuName
      Case "OfferNew".ToLower
        Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedOfferNr = 0})
        obj.OpenSelectedOffer()

			Case "SendOfferWithMail".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedOfferNr = _ClsSetting.SelectedOfferNr,
																												 .SelectedKDNr = _ClsSetting.SelectedKDNr,
																												 .SelectedZHDNr = _ClsSetting.SelectedZHDNr})
				obj.SendOffertWithMail()


			Case Else
				Exit Sub

		End Select

  End Sub

#End Region


  Sub ListAktivMandanten(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
    Dim strQuery As String = "[Get MandantListing In MainView]"

    cbo.Properties.Items.Clear()
    Try

      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      cmd.CommandType = CommandType.StoredProcedure

      Dim rFrec As SqlDataReader = cmd.ExecuteReader
      While rFrec.Read
        cbo.Properties.Items.Add(New ComboValue(rFrec("Mandantenname"), rFrec("MDNr")))

      End While

    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

    End Try

  End Sub


End Class

