
Imports SP.Infrastructure.Logging

Imports System.Data.SqlClient

Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid
Imports System.Threading

Public Class ClsMAModuls
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsSetting As New ClsMASetting
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private m_translate As TranslateValues


	Public Sub New(ByVal _setting As ClsMASetting)
		Me._ClsSetting = _setting
		m_translate = New TranslateValues

	End Sub


#Region "Funktionen für Kontextmenü..."

	''' <summary>
	''' Creates contextmenu for printbutton candidate
	''' </summary>
	''' <param name="sender"></param>
	''' <remarks></remarks>
	Sub ShowContextMenu4Print(ByVal mgr As DevExpress.XtraBars.BarManager, ByVal sender As DevExpress.XtraEditors.DropDownButton)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim strQuery As String = "[Get ContexMenuItems 4 Print In MainView]"
		Dim popupMenu1 As New DevExpress.XtraBars.PopupMenu
		'Dim mgr As New DevExpress.XtraBars.BarManager


		popupMenu1.Manager = mgr
		sender.DropDownControl = popupMenu1

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			cmd.CommandType = CommandType.StoredProcedure
			param = cmd.Parameters.AddWithValue("@ModulName", "MAPrint")
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

					ElseIf itm.Name.ToLower = "AllEmployeeForgottenZVARGB".ToLower Then
						If Not (ModulConstants.UserSecValue(108) AndAlso ModulConstants.UserSecValue(110)) Then Continue While
					End If

					If strmnuBez.StartsWith("_") OrElse strmnuBez.StartsWith("-") Then
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
			Case "mastammblatt".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr, .SelectedMANr = Me._ClsSetting.SelectedMANr})
				'obj.AddContentToPDF()
				obj.PrintMAStammblatt()

			Case "MASuvaStd".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr, .SelectedMANr = Me._ClsSetting.SelectedMANr})
				obj.PrintSuvaStdListe4SelectedEmployee()

			Case "MAZV".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr, .SelectedMANr = Me._ClsSetting.SelectedMANr})
				obj.PrintZV4SelectedEmployee()

			Case "MAArg".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr, .SelectedMANr = Me._ClsSetting.SelectedMANr})
				obj.PrintARG4SelectedEmployee()

			Case "AllEmployeeForgottenZVARGB".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr})
				obj.PrintEmployeeForgottenZVARGB()

			Case "MALO".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr, .SelectedLONr = 0, .SelectedMANr = Me._ClsSetting.SelectedMANr})
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
			param = cmd.Parameters.AddWithValue("@ModulName", "MANew")
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

					If itm.Name.ToLower = "zgNew".ToLower Then
						If Not ModulConstants.UserSecValue(350) Then Continue While
					ElseIf itm.Name.ToLower = "LONew".ToLower Then
						If Not ModulConstants.UserSecValue(550) Then Continue While
					ElseIf itm.Name.ToLower = "ProposeNew".ToLower Then
						If Not ModulConstants.UserSecValue(802) Then Continue While
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
			Case "MANew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = 0})
				obj.OpenSelectedEmployee(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ProposeNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																													 .SelectedMANr = Me._ClsSetting.SelectedMANr,
																													 .SelectedKDNr = 0,
																													 .SelectedZHDNr = 0,
																													 .SelectedVakNr = 0,
																													 .SelectedProposeNr = 0})
				obj.OpenSelectedProposeTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ESNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																													 .SelectedESNr = Nothing, .SelectedMANr = Me._ClsSetting.SelectedMANr,
																													 .SelectedKDNr = Nothing, .SelectedZHDNr = Nothing, .SelectedVakNr = Nothing, .SelectedProposeNr = Nothing})

				obj.OpenNewESForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "zgNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																													 .SelectedMANr = Me._ClsSetting.SelectedMANr,
																													 .SelectedZGNr = Nothing,
																													 .SelectedRPNr = Nothing})
				obj.OpenSelectedAdvancePayment(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "LONew".ToLower
				Dim _ClsLO As New ClsOpenModul(New ClsSetting With {.SelectedMonth = Now.Month})
				_ClsLO.OpenPayrollForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)


			Case "ContactNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																													 .SelectedMANr = Me._ClsSetting.SelectedMANr})
				obj.OpenSelectedEmployeeContactForNewEntry()

			Case "DocScan".ToLower
				Dim _ClsLO As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr})
				_ClsLO.OpenAutoRPScan()


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
