
Imports System.Data.SqlClient

Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.DatabaseAccess.Customer

Public Class ClsKDModuls
	Private Shared m_Logger As ILogger = New Logger()

	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

	Private _ClsSetting As New ClsKDSetting
	Private m_translate As TranslateValues


	Public Sub New(ByVal _setting As ClsKDSetting)
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
			param = cmd.Parameters.AddWithValue("@ModulName", "KDPrint")
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
			Case "kdstammblatt".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._ClsSetting.SelectedKDNr})
				obj.PrintKDStammblatt()

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
			param = cmd.Parameters.AddWithValue("@ModulName", "KDNew")
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
					ElseIf itm.Name.ToLower = "KDNew".ToLower Then
						If Not ModulConstants.UserSecValue(202) Then Continue While
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
			Case "kdNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = 0})
				obj.OpenSelectedCustomer(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "zhdNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._ClsSetting.SelectedKDNr,
																												 .SelectedZHDNr = Nothing})
				obj.OpenSelectedCPerson()'ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "VacancyNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedVakNr = 0,
																												 .SelectedKDNr = Me._ClsSetting.SelectedKDNr,
																												 .SelectedZHDNr = Me._ClsSetting.SelectedKDzNr,
																												 .SelectedMANr = 0})
				obj.OpenSelectedVacancyTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ProposeNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																												 .SelectedProposeNr = 0,
																												 .SelectedKDNr = Me._ClsSetting.SelectedKDNr,
																												 .SelectedZHDNr = Me._ClsSetting.SelectedKDzNr,
																												 .SelectedMANr = Nothing,
																												 .SelectedVakNr = Nothing})
				obj.OpenSelectedProposeTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ESNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedESNr = Nothing, .SelectedMANr = Nothing,
																												 .SelectedKDNr = Me._ClsSetting.SelectedKDNr, .SelectedZHDNr = Me._ClsSetting.SelectedKDzNr,
																												 .SelectedVakNr = Nothing, .SelectedProposeNr = Nothing})
				obj.CreateNewES(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "reNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedRENr = 0,
																												 .SelectedKDNr = Me._ClsSetting.SelectedKDNr})
				obj.OpenSelectedInvoice(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ContactNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._ClsSetting.SelectedKDNr,
																												 .SelectedZHDNr = Me._ClsSetting.SelectedKDzNr})
				obj.OpenSelectedCustomerContactForNewEntry()

			Case "DocScan".ToLower
				Dim _ClsLO As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr})
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
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Public Sub CreateLogToKontaktDb(ByVal contactData As ResponsiblePersonAssignedContactData)

		If contactData Is Nothing Then Return
		Dim m_CurrentContactRecordNumber As Integer?
		Dim m_UtilityUI As New SP.Infrastructure.UI.UtilityUI

		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

		contactData.ResponsiblePersonNumber = contactData.ResponsiblePersonNumber.GetValueOrDefault(0)
		contactData.ContactImportant = contactData.ContactImportant.GetValueOrDefault(False)
		contactData.ContactFinished = contactData.ContactFinished.GetValueOrDefault(False)
		contactData.MANr = contactData.MANr.GetValueOrDefault(0)
		contactData.VacancyNumber = contactData.VacancyNumber.GetValueOrDefault(0)
		contactData.ProposeNr = contactData.ProposeNr.GetValueOrDefault(0)

		contactData.ChangedFrom = ModulConstants.UserData.UserFullName
		contactData.ChangedOn = DateTime.Now
		contactData.UsNr = ModulConstants.UserData.UserNr

		Dim isNewContact = (contactData.ID = 0)

		Dim success As Boolean = True


		' Insert or update contact
		If isNewContact Then
			contactData.CreatedUserNumber = ModulConstants.UserData.UserNr
			success = success AndAlso m_CustomerDatabaseAccess.AddResponsiblePersonContactAssignment(contactData)

			If success Then
				m_CurrentContactRecordNumber = contactData.RecordNumber
			End If

		Else
			contactData.ChangedUserNumber = ModulConstants.UserData.UserNr
			success = success AndAlso m_CustomerDatabaseAccess.UpdateResponsiblePersonAssignedContactData(contactData)
		End If


		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Kontaktdaten konnten nicht gespeichert werden."))

		Else

			If m_CurrentContactRecordNumber > 0 Then
				Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.ContactRecordNumber = m_CurrentContactRecordNumber, .SelectedKDNr = contactData.CustomerNumber,
																			 .SelectedZHDNr = contactData.ResponsiblePersonNumber.GetValueOrDefault(0)})
				_ClsKD.OpenSelectedCustomerContact()
			End If

		End If


	End Sub



End Class

