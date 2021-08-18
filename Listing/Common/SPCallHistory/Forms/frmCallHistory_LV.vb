
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.ComponentModel
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.KD.CPersonMng.UI
Imports SP.Infrastructure.Logging

Public Class frmCallHistory_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML
	Private m_utility As Utilities
	Private m_md As Mandant
	Private m_ShowcustomerinColor As Boolean


	Enum ListArt
		CallDb = 0
		customer = 1
		employee = 2
	End Enum


	Public Property RecCount As Integer
	Private Property Sql2Open As String
	Private Property QueryArt As ListArt


#Region "Constructor..."

	Public Sub New(ByVal _sql As String, ByVal _listart As ListArt)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()
		m_md = New Mandant

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		Me.Sql2Open = _sql

		Me.QueryArt = _listart
		If Me.QueryArt = ListArt.CallDb Then
			ResetGridData4Call()
		ElseIf Me.QueryArt = ListArt.customer Then
			ResetGridData4CustomerContact()
		ElseIf Me.QueryArt = ListArt.employee Then
			ResetGridData4EmployeeContact()

		End If

		m_ShowcustomerinColor = ShowCustomerRecordsInColor

	End Sub

#End Region

#Region "Private property"

	Private ReadOnly Property ShowCustomerRecordsInColor() As Boolean
		Get
			Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim mdNumber As Integer = 0
			Dim userNumber As Integer = 0
			mdNumber = ClsDataDetail.MDData.MDNr
			userNumber = ClsDataDetail.UserData.UserNr

			Dim UserXMLFileName = m_md.GetSelectedMDUserProfileXMLFilename(mdNumber, userNumber)
			Dim value As Boolean? = StrToBool(m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/showcustomerrecordsincolor", FORM_XML_MAIN_KEY)))


			Return value
		End Get

	End Property

#End Region


	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As FoundedCallData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedCallData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property


#Region "Open Database"


	Function GetCallDbData4Show() As IEnumerable(Of FoundedCallData)
		Dim result As List(Of FoundedCallData) = Nothing
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedCallData)

				While reader.Read()
					Dim overviewData As New FoundedCallData

					overviewData.zhdnr = CInt(m_utility.SafeGetInteger(reader, "zhdnr", 0))
					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", 0))
					overviewData.KDNr = CInt(m_utility.SafeGetInteger(reader, "KDNr", 0))

					overviewData.employeename = String.Format("{0}", m_utility.SafeGetString(reader, "kandidatenname"))
					overviewData.customername = String.Format("{0}", m_utility.SafeGetString(reader, "Firmenname"))
					overviewData.responsiblename = String.Format("{0}", m_utility.SafeGetString(reader, "zuständigeperson"))
					overviewData.berater = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "USNachname"), m_utility.SafeGetString(reader, "USVorname"))
					overviewData.zeitpunkt = CType(String.Format("{0}", m_utility.SafeGetDateTime(reader, "zeitpunkt", Nothing)), Date?)
					overviewData.recinfo = String.Format("{0}", m_utility.SafeGetString(reader, "info"))
					overviewData.recipient = String.Format("{0}", m_utility.SafeGetString(reader, "CalledTo"))

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

	Function GetCustomerContactDbData4Show() As IEnumerable(Of FoundedCallData)
		Dim result As List(Of FoundedCallData) = Nothing
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open
		If String.IsNullOrWhiteSpace(sql) Then Return Nothing

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedCallData)

				While reader.Read()
					Dim overviewData As New FoundedCallData

					overviewData.recnr = CInt(m_utility.SafeGetInteger(reader, "recnr", 0))
					overviewData.zhdnr = CInt(m_utility.SafeGetInteger(reader, "zhdnr", 0))
					overviewData.KDNr = CInt(m_utility.SafeGetInteger(reader, "KDNr", 0))
					overviewData.fproperty = m_utility.SafeGetInteger(reader, "FProperty", 0)


					overviewData.customername = String.Format("{0}", m_utility.SafeGetString(reader, "Firmenname"))
					overviewData.responsiblename = String.Format("{0}", m_utility.SafeGetString(reader, "zuständigeperson"))

					overviewData.contactsubject = String.Format("{0}", m_utility.SafeGetString(reader, "Aufgabenbetreff"))
					overviewData.contactdate = CType(String.Format("{0}", m_utility.SafeGetDateTime(reader, "datum", Nothing)), Date?)
					overviewData.contacttype = String.Format("{0}", m_utility.SafeGetString(reader, "kontakttype"))
					overviewData.contactdescription = String.Format("{0}", m_utility.SafeGetString(reader, "Kontaktbeschreibung"))

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

	Function GetEmployeeContactDbData4Show() As IEnumerable(Of FoundedCallData)
		Dim result As List(Of FoundedCallData) = Nothing
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedCallData)

				While reader.Read()
					Dim overviewData As New FoundedCallData

					overviewData.recnr = CInt(m_utility.SafeGetInteger(reader, "recnr", 0))
					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", 0))

					overviewData.employeename = String.Format("{0}", m_utility.SafeGetString(reader, "Kandidatenname"))

					overviewData.contactsubject = String.Format("{0}", m_utility.SafeGetString(reader, "Aufgabenbetreff"))
					overviewData.contactdate = CType(String.Format("{0}", m_utility.SafeGetDateTime(reader, "datum", Nothing)), Date?)
					overviewData.contacttype = String.Format("{0}", m_utility.SafeGetString(reader, "kontakttype"))
					overviewData.contactdescription = String.Format("{0}", m_utility.SafeGetString(reader, "Kontaktbeschreibung"))

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


#End Region


#Region "Load Data"


	Private Function LoadCallList() As Boolean

		Dim listOfEmployees = GetCallDbData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedCallData With
																						 {.zhdnr = person.zhdnr,
																							.MANr = person.MANr,
																							.KDNr = person.KDNr,
																							.berater = person.berater,
																							.recipient = person.recipient,
																							.recinfo = person.recinfo,
																							.zeitpunkt = person.zeitpunkt,
																							.employeename = person.employeename,
																							.customername = person.customername,
																							.responsiblename = person.responsiblename
																						}).ToList()

		Dim listDataSource As BindingList(Of FoundedCallData) = New BindingList(Of FoundedCallData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Function LoadCustomerContactList() As Boolean

		Dim listOfEmployees = GetCustomerContactDbData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedCallData With
					 {.recnr = person.recnr,
						.zhdnr = person.zhdnr,
						.KDNr = person.KDNr,
						.fproperty = person.fproperty,
						.contactdate = person.contactdate,
						.contacttype = person.contacttype,
						.customername = person.customername,
						.responsiblename = person.responsiblename,
						.contactdescription = person.contactdescription,
						.contactsubject = person.contactsubject
					}).ToList()

		Dim listDataSource As BindingList(Of FoundedCallData) = New BindingList(Of FoundedCallData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Function LoadEmployeeContactList() As Boolean

		Dim listOfEmployees = GetEmployeeContactDbData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedCallData With
																						 {.recnr = person.recnr,
																							.MANr = person.MANr,
																							.contactdate = person.contactdate,
																							.contacttype = person.contacttype,
																							.employeename = person.employeename,
																							.contactdescription = person.contactdescription,
																							.contactsubject = person.contactsubject
																						}).ToList()

		Dim listDataSource As BindingList(Of FoundedCallData) = New BindingList(Of FoundedCallData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


#End Region



#Region "Reset Column"


	Private Sub ResetGridData4Call()

		gvRP.Columns.Clear()

		Dim columnzhdNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzhdNumber.Caption = "zhdnr"
		columnzhdNumber.Name = "zhdnr"
		columnzhdNumber.FieldName = "zhdnr"
		columnzhdNumber.Visible = False
		gvRP.Columns.Add(columnzhdNumber)

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = "maNr"
		columnMANr.Name = "manr"
		columnMANr.FieldName = "manr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.Caption = "KDNr"
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Visible = False
		gvRP.Columns.Add(columnKDNr)

		Dim columnAdvoiser As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAdvoiser.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAdvoiser.Caption = m_xml.GetSafeTranslationValue("BeraterIn")
		columnAdvoiser.Name = "berater"
		columnAdvoiser.FieldName = "berater"
		columnAdvoiser.Visible = True
		gvRP.Columns.Add(columnAdvoiser)


		Dim columnrecipient As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecipient.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnrecipient.Caption = m_xml.GetSafeTranslationValue("Telefoniert an")
		columnrecipient.Name = "recipient"
		columnrecipient.FieldName = "recipient"
		columnrecipient.Visible = True
		gvRP.Columns.Add(columnrecipient)

		Dim columnrecinfo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecinfo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnrecinfo.Caption = m_xml.GetSafeTranslationValue("Info")
		columnrecinfo.Name = "recinfo"
		columnrecinfo.FieldName = "recinfo"
		columnrecinfo.Visible = True
		gvRP.Columns.Add(columnrecinfo)

		Dim columnzeitpunkt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzeitpunkt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnzeitpunkt.Caption = m_xml.GetSafeTranslationValue("Zeitpunkt")
		columnzeitpunkt.Name = "zeitpunkt"
		columnzeitpunkt.FieldName = "zeitpunkt"
		columnzeitpunkt.Visible = True
		columnzeitpunkt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		gvRP.Columns.Add(columnzeitpunkt)

		Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployee.Caption = m_xml.GetSafeTranslationValue("Kandidatenname")
		columnEmployee.Name = "employeename"
		columnEmployee.FieldName = "employeename"
		columnEmployee.Visible = True
		gvRP.Columns.Add(columnEmployee)

		Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer.Caption = m_xml.GetSafeTranslationValue("Firmenname")
		columnCustomer.Name = "customername"
		columnCustomer.FieldName = "customername"
		columnCustomer.Visible = True
		gvRP.Columns.Add(columnCustomer)

		Dim columnresponsible As New DevExpress.XtraGrid.Columns.GridColumn()
		columnresponsible.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnresponsible.Caption = m_xml.GetSafeTranslationValue("Zuständige Person")
		columnresponsible.Name = "responsiblename"
		columnresponsible.FieldName = "responsiblename"
		columnresponsible.Visible = True
		gvRP.Columns.Add(columnresponsible)

		grdRP.DataSource = Nothing

	End Sub

	Private Sub ResetGridData4CustomerContact()

		gvRP.Columns.Clear()

		Dim columnzhdNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzhdNumber.Caption = "zhdnr"
		columnzhdNumber.Name = "zhdnr"
		columnzhdNumber.FieldName = "zhdnr"
		columnzhdNumber.Visible = False
		gvRP.Columns.Add(columnzhdNumber)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.Caption = "kdNr"
		columnKDNr.Name = "kdnr"
		columnKDNr.FieldName = "kdnr"
		columnKDNr.Visible = False
		gvRP.Columns.Add(columnKDNr)

		Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer.Caption = m_xml.GetSafeTranslationValue("Firmenname")
		columnCustomer.Name = "customername"
		columnCustomer.FieldName = "customername"
		columnCustomer.Visible = True
		gvRP.Columns.Add(columnCustomer)

		Dim columnresponsible As New DevExpress.XtraGrid.Columns.GridColumn()
		columnresponsible.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnresponsible.Caption = m_xml.GetSafeTranslationValue("Zuständige Person")
		columnresponsible.Name = "responsiblename"
		columnresponsible.FieldName = "responsiblename"
		columnresponsible.Visible = True
		gvRP.Columns.Add(columnresponsible)

		Dim columnzeitpunkt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzeitpunkt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnzeitpunkt.Caption = m_xml.GetSafeTranslationValue("Datum")
		columnzeitpunkt.Name = "contactdate"
		columnzeitpunkt.FieldName = "contactdate"
		columnzeitpunkt.Visible = True
		columnzeitpunkt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		gvRP.Columns.Add(columnzeitpunkt)

		Dim columncontacttype As New DevExpress.XtraGrid.Columns.GridColumn()
		columncontacttype.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncontacttype.Caption = m_xml.GetSafeTranslationValue("Kontaktart")
		columncontacttype.Name = "contacttype"
		columncontacttype.FieldName = "contacttype"
		columncontacttype.Visible = True
		gvRP.Columns.Add(columncontacttype)

		Dim columnsubject As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsubject.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnsubject.Caption = m_xml.GetSafeTranslationValue("Betreff")
		columnsubject.Name = "contactsubject"
		columnsubject.FieldName = "contactsubject"
		columnsubject.Visible = True
		gvRP.Columns.Add(columnsubject)

		Dim columnDespcription As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDespcription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDespcription.Caption = m_xml.GetSafeTranslationValue("Beschreibung")
		columnDespcription.Name = "contactdescription"
		columnDespcription.FieldName = "contactdescription"
		columnDespcription.Visible = True
		gvRP.Columns.Add(columnDespcription)


		grdRP.DataSource = Nothing

	End Sub

	Private Sub ResetGridData4EmployeeContact()

		gvRP.Columns.Clear()

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = "maNr"
		columnMANr.Name = "manr"
		columnMANr.FieldName = "manr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployee.Caption = m_xml.GetSafeTranslationValue("Kandidatenname")
		columnEmployee.Name = "employeename"
		columnEmployee.FieldName = "employeename"
		columnEmployee.Visible = True
		gvRP.Columns.Add(columnEmployee)

		Dim columnzeitpunkt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzeitpunkt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnzeitpunkt.Caption = m_xml.GetSafeTranslationValue("Datum")
		columnzeitpunkt.Name = "contactdate"
		columnzeitpunkt.FieldName = "contactdate"
		columnzeitpunkt.Visible = True
		columnzeitpunkt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		gvRP.Columns.Add(columnzeitpunkt)

		Dim columncontacttype As New DevExpress.XtraGrid.Columns.GridColumn()
		columncontacttype.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncontacttype.Caption = m_xml.GetSafeTranslationValue("Kontaktart")
		columncontacttype.Name = "contacttype"
		columncontacttype.FieldName = "contacttype"
		columncontacttype.Visible = True
		gvRP.Columns.Add(columncontacttype)

		Dim columnsubject As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsubject.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnsubject.Caption = m_xml.GetSafeTranslationValue("Betreff")
		columnsubject.Name = "contactsubject"
		columnsubject.FieldName = "contactsubject"
		columnsubject.Visible = True
		gvRP.Columns.Add(columnsubject)

		Dim columnDespcription As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDespcription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDespcription.Caption = m_xml.GetSafeTranslationValue("Beschreibung")
		columnDespcription.Name = "contactdescription"
		columnDespcription.FieldName = "contactdescription"
		columnDespcription.Visible = True
		gvRP.Columns.Add(columnDespcription)

		grdRP.DataSource = Nothing

	End Sub


#End Region

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_xml.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue(Me.bsiInfo.Caption)
			bbiPrintList.Caption = m_xml.GetSafeTranslationValue(Me.bbiPrintList.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

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

		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		TranslateControls()
		Try

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

		Catch ex As Exception

		End Try

		Try

			If Me.QueryArt = ListArt.CallDb Then
				LoadCallList()
			ElseIf Me.QueryArt = ListArt.customer Then
				LoadCustomerContactList()
			ElseIf Me.QueryArt = ListArt.employee Then
				LoadEmployeeContactList()

			End If


			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.RecCount)

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick

		Catch ex As Exception

		End Try

	End Sub


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedCallData)

				Select Case column.Name.ToLower
					Case "manr"
						If viewData.MANr > 0 Then loadSelectedEmployee(viewData.MANr)

					Case "kdnr"
						If viewData.KDNr > 0 Then loadSelectedCustomer(viewData.KDNr)

					Case "zhdnr"
						If viewData.zhdnr > 0 Then LoadCustomerResponsible(viewData.KDNr, viewData.zhdnr)

					Case Else
						If Me.QueryArt = ListArt.CallDb Then

						ElseIf Me.QueryArt = ListArt.customer Then
							If viewData.recnr > 0 Then LoadCustomerContact(viewData.KDNr, viewData.zhdnr, viewData.recnr)
						ElseIf Me.QueryArt = ListArt.employee Then
							If viewData.recnr > 0 Then LoadEmployeeContact(viewData.MANr, viewData.recnr)

						End If



				End Select

			End If

		End If

	End Sub

	''' <summary>
	'''  Handles RowStyle event of main grid view.
	''' </summary>
	Private Sub OngvRPn_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvRP.RowStyle

		If Not m_ShowcustomerinColor Then Return
		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvRP.GetRow(e.RowHandle), FoundedCallData)

			If rowData.fproperty > 0 Then
				e.Appearance.ForeColor = ColorTranslator.FromWin32(rowData.fproperty)
			End If

		End If

	End Sub


	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub


	Sub loadSelectedEmployee(ByVal Employeenumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEmployeeMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, Employeenumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

	Sub loadSelectedCustomer(ByVal _iKDNr As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenCustomerMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, _iKDNr)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Sub LoadCustomerResponsible(ByVal _iKDNr As Integer, ByVal _iKDZHDNr As Integer)

		Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr))

		If (responsiblePersonsFrom.LoadResponsiblePersonData(_iKDNr, _iKDZHDNr)) Then
			responsiblePersonsFrom.Show()
		End If

	End Sub

	Sub LoadCustomerContact(ByVal _iKDNr As Integer?, ByVal _iKDZHDNr As Integer?, ByVal recordNumber As Integer)

		Try
			Dim kontakte = New SP.KD.KontaktMng.frmContacts(CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr))
			Dim customerNumber As Integer = CInt(_iKDNr)
			Dim zhdnr As Integer? = _iKDZHDNr
			If customerNumber = 0 Then Return

			If _iKDZHDNr = 0 Then zhdnr = Nothing
			If (kontakte.ActivateNewContactDataMode(customerNumber, zhdnr, Nothing, Nothing)) Then
				kontakte.LoadContactData(customerNumber, zhdnr, recordNumber, Nothing)
				kontakte.Show()
				kontakte.BringToFront()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Sub LoadEmployeeContact(ByVal employeeNumber As Integer, ByVal recordNumber As Integer)

		Try
			Dim kontakte = New SP.MA.KontaktMng.frmContacts(CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr))
			If employeeNumber = 0 Then Return

			If (kontakte.ActivateNewContactDataMode(employeeNumber, Nothing, Nothing)) Then
				kontakte.LoadContactData(employeeNumber, recordNumber, Nothing)
				kontakte.Show()
				kontakte.BringToFront()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub


#Region "Helpers"

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		ElseIf str = "1" Then
			Return True

		End If

		Boolean.TryParse(str, result)

		Return result
	End Function


#End Region


End Class