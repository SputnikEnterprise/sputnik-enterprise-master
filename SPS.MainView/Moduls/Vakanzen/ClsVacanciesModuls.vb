
Imports System.Data.SqlClient

Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid
Imports System.Threading
Imports SP.KD.CustomerMng.UI
Imports SP.Infrastructure.Logging



Public Class ClsVacanciesModuls
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsSetting As New ClsVakSetting
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	Public Sub New(ByVal _setting As ClsVakSetting)
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
			param = cmd.Parameters.AddWithValue("@ModulName", "VakPrint")
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
			Case "Vakstammblatt".ToLower
				Dim _Setting As New SPS.Listing.Print.Utility.ClsLLVakSearchPrintSetting With {.DbConnString2Open = ModulConstants.MDData.MDDbConn,
																																												 .SQL2Open = "[Get SelectedVakData For Print Stammblatt]",
																																												 .JobNr2Print = "19.0.1",
																																												 .VakNr2Print = Me._ClsSetting.SelectedVakNr,
																																												 .ListFilterBez = New List(Of String)(New String() {"",
																																		"",
																																		"",
																																		""})}
				Dim obj As New SPS.Listing.Print.Utility.VakSearchListing.ClsPrintVakSearchList(_Setting)
				Dim strResult As String = obj.PrintVakTpl_1(False, Me._ClsSetting.SelectedVakNr)


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
			param = cmd.Parameters.AddWithValue("@ModulName", "VakNew")
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

					If itm.Name.ToLower = "ESNew".ToLower Then
						If Not ModulConstants.UserSecValue(251) Then Continue While
					ElseIf itm.Name.ToLower = "VacancyNew".ToLower Then
						If Not ModulConstants.UserSecValue(702) Then Continue While
					ElseIf itm.Name.ToLower = "ProposeNew".ToLower Then
						If Not ModulConstants.UserSecValue(802) Then Continue While
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
			Case "VakNew".ToLower
				If Not Me._ClsSetting.SelectedKDNr.HasValue Then Me._ClsSetting.SelectedKDNr = 0
				If Not Me._ClsSetting.SelectedKDzNr.HasValue Then Me._ClsSetting.SelectedKDzNr = 0
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																			.SelectedVakNr = 0,
																			.SelectedKDNr = Me._ClsSetting.SelectedKDNr,
																			.SelectedZHDNr = Me._ClsSetting.SelectedKDzNr})
				obj.OpenSelectedVacancyTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ProposeNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																													 .SelectedVakNr = Me._ClsSetting.SelectedVakNr,
																													 .SelectedKDNr = Me._ClsSetting.SelectedKDNr,
																													 .SelectedZHDNr = Me._ClsSetting.SelectedKDzNr})
				obj.OpenSelectedProposeTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ESNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Nothing,
															 .SelectedESNr = Nothing, .SelectedMANr = Nothing,
															 .SelectedKDNr = If(Me._ClsSetting.SelectedKDNr = 0, Nothing, Me._ClsSetting.SelectedKDNr),
															 .SelectedZHDNr = If(Me._ClsSetting.SelectedKDzNr = 0, Nothing, Me._ClsSetting.SelectedKDzNr),
															 .SelectedVakNr = Me._ClsSetting.SelectedVakNr,
															 .SelectedProposeNr = Nothing})
				obj.CreateNewES(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)


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

	Sub ShowCallForm(ByVal strValue As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim iKDNr As Integer = Me._ClsSetting.SelectedKDNr

		Try

			' TODO:
			'Dim _ClsSystem As New Translate4_Net.ClsMain_Net
			'_ClsSystem.RunTapi(strValue, iKDNr, 0, 0, 0, 0)

		Catch ex As Exception
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

		End Try

	End Sub



End Class

