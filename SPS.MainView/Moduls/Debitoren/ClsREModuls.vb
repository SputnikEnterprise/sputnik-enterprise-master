
Imports System.Data.SqlClient

Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid
Imports System.Threading

Imports SP.Infrastructure.Logging
Imports SP.KD.CustomerMng.UI
Imports SP.KD.CPersonMng.UI
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure.UI

Public Class ClsREModuls
	Private Shared m_Logger As ILogger = New Logger()

	Private m_translate As TranslateValues
	Private m_utilityUI As UtilityUI

	Private _ClsSetting As New ClsRESetting
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private m_InvoiceData As FoundedCustomerBillData


	Public Sub New(ByVal _setting As ClsRESetting, invoiceData As FoundedCustomerBillData)
		m_translate = New TranslateValues
		m_utilityUI = New UtilityUI

		Me._ClsSetting = _setting
		m_InvoiceData = invoiceData

	End Sub


#Region "Funktionen für Kontextmenü..."

	''' <summary>
	''' Creates contextmenu for printbutton candidate
	''' </summary>
	''' <param name="sender"></param>
	''' <remarks></remarks>
	Sub ShowContextMenu4Print(ByVal mgr As DevExpress.XtraBars.BarManager, sender As DevExpress.XtraEditors.DropDownButton)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim strQuery As String = "[Get ContexMenuItems 4 Print In MainView]"
		Dim popupMenu1 As New DevExpress.XtraBars.PopupMenu

		popupMenu1.Manager = mgr
		sender.DropDownControl = popupMenu1

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			cmd.CommandType = CommandType.StoredProcedure
			param = cmd.Parameters.AddWithValue("@ModulName", "OPPrint")
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

					If itm.Name.ToLower = "printop".ToLower Then
						If Not ModulConstants.UserSecValue(406) Then Continue While
					ElseIf itm.Name.ToLower = "printmoreop".ToLower Then
						If Not ModulConstants.UserSecValue(406) Then Continue While
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
			Case "printop".ToLower
				If m_InvoiceData Is Nothing Then Return
				Dim printOpenAmount As Boolean = m_InvoiceData.reart1 <> "G" AndAlso m_InvoiceData.reart1 <> "R" AndAlso m_InvoiceData.bezahlt.GetValueOrDefault(0) > 0
				If printOpenAmount AndAlso m_InvoiceData.betragink.GetValueOrDefault(0) > m_InvoiceData.bezahlt.GetValueOrDefault(0) Then
					Dim msg = m_translate.GetSafeTranslationValue("Möchten Sie die Rechnung mit offener Betrag drucken?")
					printOpenAmount = m_utilityUI.ShowYesNoDialog(msg, m_translate.GetSafeTranslationValue("Rechnung drucken"), MessageBoxDefaultButton.Button2)
				End If

				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = _ClsSetting.SelectedMDNr, .SelectedRENr = _ClsSetting.SelectedRENr, .PrintOpenAmount = printOpenAmount})
				obj.PrintSelectedOPRecord()

			Case "printmoreop".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = _ClsSetting.SelectedMDNr, .SelectedRENr = _ClsSetting.SelectedRENr})
				obj.PrintMoreOPRecords()

			Case Else
				Exit Sub

		End Select

	End Sub

	''' <summary>
	''' Creates contextmenu for newbutton candidate
	''' </summary>
	''' <param name="sender"></param>
	''' <remarks></remarks>
	Sub ShowContextMenu4New(ByVal mgr As DevExpress.XtraBars.BarManager, sender As DevExpress.XtraEditors.DropDownButton)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim strQuery As String = "[Get ContexMenuItems 4 NewRec In MainView]"
		Dim popupMenu1 As New DevExpress.XtraBars.PopupMenu
		If ModulConstants.MDData.ClosedMD Then Return

		popupMenu1.Manager = mgr
		sender.DropDownControl = popupMenu1

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			cmd.CommandType = CommandType.StoredProcedure
			param = cmd.Parameters.AddWithValue("@ModulName", "OPNew")
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


					If itm.Name.ToLower = "OPAutoNew".ToLower Then
						If Not ModulConstants.UserSecValue(402) Then Continue While
					ElseIf itm.Name.ToLower = "OPIndNew".ToLower Then
						If Not ModulConstants.UserSecValue(401) Then Continue While
					ElseIf itm.Name.ToLower = "OPZENew".ToLower OrElse itm.Name.ToLower = "OPZENewWithNoCustomer".ToLower Then
						If Not ModulConstants.UserSecValue(501) Then Continue While
					ElseIf itm.Name.ToLower = "CreateBESR".ToLower Then
						If Not ModulConstants.UserSecValue(503) Then Continue While
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
			Case "OPAutoNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr})
				obj.OpenNewREAutoForm()

			Case "OPIndNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																												 .SelectedRENr = Nothing,
																												 .SelectedKDNr = Me._ClsSetting.SelectedKDNr})
				obj.OpenNewInvoice(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "OPZENew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr,
																												 .SelectedRENr = Me._ClsSetting.SelectedRENr,
																												 .SelectedKDNr = Me._ClsSetting.SelectedKDNr})
				' TODO: FA. 
				'obj.OpenNewZEForm()
				obj.OpenNewPaymentForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "OPZENewWithNoCustomer".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr,
																												 .SelectedRENr = 0,
																												 .SelectedKDNr = 0})
				' TODO: FA. 
				'obj.OpenNewZEForm()
				obj.OpenNewPaymentForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "CreateBESR".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr})
				obj.OpenNewBESRForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)


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

