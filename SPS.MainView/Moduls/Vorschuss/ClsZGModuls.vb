
Imports System.Data.SqlClient

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports DevExpress.XtraGrid.Views.Grid
Imports System.Threading
Imports SP.Infrastructure.Logging



Public Class ClsZGModuls
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsSetting As New ClsZGSetting



	Public Sub New(ByVal _setting As ClsZGSetting)
		Me._ClsSetting = _setting
	End Sub


#Region "Funktionen für Kontextmenü..."

	''' <summary>
	''' Creates contextmenu for printbutton candidate
	''' </summary>
	''' <param name="sender"></param>
	''' <remarks></remarks>
	Sub ShowContextMenu4Print(sender As DevExpress.XtraEditors.DropDownButton)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim strQuery As String = "[Get ContexMenuItems 4 Print In MainView]"
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
			param = cmd.Parameters.AddWithValue("@ModulName", "ZGPrint")
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

					If itm.Name.ToLower = "MAZV".ToLower Then
						If Not ModulConstants.UserSecValue(108) Then Continue While
					ElseIf itm.Name.ToLower = "MAArg".ToLower Then
						If Not ModulConstants.UserSecValue(110) Then Continue While
					ElseIf itm.Name.ToLower = "MALO".ToLower Then
						If Not ModulConstants.UserSecValue(554) Then Continue While
					ElseIf itm.Name.ToLower = "MALOMore".ToLower Then
						If Not ModulConstants.UserSecValue(554) Then Continue While
					ElseIf itm.Name.ToLower = "mastammblatt".ToLower Then
						If Not ModulConstants.UserSecValue(104) Then Continue While
					ElseIf itm.Name.ToLower = "MASuvaStd".ToLower Then
						If Not ModulConstants.UserSecValue(106) Then Continue While
					End If

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

		Select Case strMnuName
			Case "MALO".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr,
																														 .SelectedLONr = Me._ClsSetting.SelectedLONr.GetValueOrDefault(0),
																														 .SelectedMANr = Me._ClsSetting.SelectedMANr})
				If Me._ClsSetting.SelectedLONr.GetValueOrDefault(0) = 0 Then
					obj.PrintMALO()
				Else
					obj.PrinSelectedtMAPayroll()
				End If

			Case "MALOMore".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr,
																														 .SelectedLONr = 0,
																														 .SelectedMANr = Me._ClsSetting.SelectedMANr})
				obj.PrintMALO()


			Case Else
				Exit Sub

		End Select

	End Sub

	''' <summary>
	''' Creates contextmenu for newbutton candidate
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
			param = cmd.Parameters.AddWithValue("@ModulName", "ZGNew")
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

					If itm.Name.ToLower = "zgNew".ToLower OrElse itm.Name.ToLower = "ZGNewWithNoEmployee".ToLower Then
						If Not ModulConstants.UserSecValue(350) Then Continue While
					ElseIf itm.Name.ToLower = "DTANew".ToLower Then
						If Not ModulConstants.UserSecValue(562) Then Continue While
					ElseIf itm.Name.ToLower = "CloseMonth".ToLower Then
						If Not ModulConstants.UserSecValue(568) Then Continue While
					ElseIf itm.Name.ToLower = "LONew".ToLower Then
						If Not ModulConstants.UserSecValue(550) Then Continue While
					ElseIf itm.Name.ToLower = "ESNew".ToLower Then
						If Not ModulConstants.UserSecValue(251) Then Continue While
					End If


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
			Case "zgNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr,
																														 .SelectedMANr = Me._ClsSetting.SelectedMANr,
																														 .SelectedZGNr = Nothing,
																														 .SelectedRPNr = Nothing})

				obj.OpenSelectedAdvancePayment(Me._ClsSetting.SelectedMDNr, ModulConstants.UserData.UserNr)

			Case "ZGNewWithNoEmployee".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr,
																														 .SelectedMANr = Nothing,
																														 .SelectedZGNr = Nothing,
																														 .SelectedRPNr = Nothing})
				obj.OpenSelectedAdvancePayment(Me._ClsSetting.SelectedMDNr, ModulConstants.UserData.UserNr)

			Case "DTANew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr,
																 .SelectedMANr = Me._ClsSetting.SelectedMANr,
																 .SelectedZGNr = 0})
				obj.OpenNewDTAForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)


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
			MsgBox(ex.Message, MsgBoxStyle.Critical, strMethodeName)

		End Try

	End Sub


End Class
