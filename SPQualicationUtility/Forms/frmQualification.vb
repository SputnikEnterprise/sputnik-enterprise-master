

Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Controls
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

Imports System.Data.SqlClient
Imports DevExpress.XtraBars
Imports DevExpress.Utils.Menu
Imports System.Drawing
Imports DevExpress.XtraGrid.Columns
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid


Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath.ClsProgPath
Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports SP.DatabaseAccess.TableSetting

Public Class frmQualification


#Region "private consts"

	Private Const JOB_ART As String = "{0} Berufsdatenbank"
	Private Const ITEM_SEPRATOR As String = "|"

#End Region

#Region "private fields"

	Protected m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Protected m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Protected m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Protected m_TablesettingDatabaseAccess As ITablesDatabaseAccess


	'Private liSelectedValue2Transfer As Dictionary(Of String, String)
	Private m_LastSelectedDb As Short = 0
	'Private _bAllowedMultiSelect As Boolean = False

	Private m_Geder As String
	Private m_CheckEditCompleted As RepositoryItemCheckEdit
	Private m_Mandant As Mandant
	Protected m_UtilityUI As UtilityUI


#End Region


#Region "Public Properties"

	Public Property GetSelectedData As String
	Public Property SelectMultirecords As Boolean

#End Region


#Region "Contructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_UtilityUI = New UtilityUI
		m_Mandant = New Mandant

		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(m_InitializationData.MDData.MDDbConn, _setting.UserData.UserLanguage)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If Not String.IsNullOrWhiteSpace(strStyleName) Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			If My.Settings.iBranchesHeight > 0 Then Me.Height = My.Settings.iBranchesHeight
			If My.Settings.iBranchesWidth > 0 Then Me.Width = My.Settings.iBranchesWidth
			If My.Settings.frmBranchesLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmBranchesLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))

		End Try

		' Completed symbol
		m_CheckEditCompleted = CType(grdQualifikation.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		m_CheckEditCompleted.PictureChecked = My.Resources.warning_16x16
		m_CheckEditCompleted.PictureUnchecked = Nothing
		m_CheckEditCompleted.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		TranslateControls()

		'AddHandler gvSelectedrec.InitNewRow, AddressOf grdSelectedrec_InitNewRow

	End Sub

#End Region


#Region "public Methodes"

	Public Function LoadQualificationData(ByVal gender As String) As Boolean
		Dim result As Boolean = True

		m_Geder = gender
		Me.m_LastSelectedDb = My.Settings.iLastSelect
		If m_LastSelectedDb = 1 Then m_LastSelectedDb = 0
		Reset()
		If m_LastSelectedDb = 0 Then
			LoadLocalQualificationData(m_Geder, m_InitializationData.UserData.UserLanguage)

		ElseIf m_LastSelectedDb = 1 Then
			LoadSecoQualificationData(m_Geder, m_InitializationData.UserData.UserLanguage)

		ElseIf m_LastSelectedDb = 2 Then
			LoadBGBQualificationData(m_Geder, m_InitializationData.UserData.UserLanguage)

		ElseIf m_LastSelectedDb = 3 Then
			LoadHBBQualificationData(m_Geder, m_InitializationData.UserData.UserLanguage)

		ElseIf m_LastSelectedDb = 4 Then
			LoadJobroomQualificationData(m_Geder, m_InitializationData.UserData.UserLanguage)

		End If

		Return result
	End Function

#End Region


#Region "Private Properties"

	Private ReadOnly Property SelectedRecord As BindingList(Of JobData)
		Get
			Dim Result = New BindingList(Of JobData)

			Try

				Dim gvRP = TryCast(grdQualifikation.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvRP Is Nothing) Then

					Dim selectedRows = gvRP.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim job = CType(gvRP.GetRow(selectedRows(0)), JobData)

						Result.Add(job)
						Return Result
					End If

				End If

			Catch ex As Exception

			End Try

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property GetSelectedQualificatioinData() As BindingList(Of JobData)
		Get

			Dim result As BindingList(Of JobData) = Nothing
			Try
				grdQualifikation.RefreshDataSource()
				Dim printList = CType(grdQualifikation.DataSource, List(Of JobData))
				Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

				result = New BindingList(Of JobData)

				For Each receiver In sentList
					result.Add(receiver)
				Next

				Return result

			Catch ex As Exception

			End Try

			Return Nothing
		End Get

	End Property

#End Region


	Private Sub Reset()
		ResetQualifikationGrid()
	End Sub

	Private Sub ResetQualifikationGrid()

		gvQualifikation.OptionsView.ShowIndicator = False
		gvQualifikation.OptionsView.ShowAutoFilterRow = True
		gvQualifikation.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvQualifikation.OptionsView.ShowFooter = False
		gvQualifikation.OptionsBehavior.Editable = True
		gvQualifikation.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvQualifikation.Columns.Clear()

		If SelectMultirecords Then
			Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSelectedRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnSelectedRec.OptionsColumn.AllowEdit = True
			columnSelectedRec.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
			columnSelectedRec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnSelectedRec.AppearanceHeader.Options.UseTextOptions = True
			columnSelectedRec.Name = "SelectedRec"
			columnSelectedRec.FieldName = "SelectedRec"
			columnSelectedRec.Visible = True
			columnSelectedRec.Width = 20
			gvQualifikation.Columns.Add(columnSelectedRec)
		End If

		Dim columnCode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCode.Caption = m_Translate.GetSafeTranslationValue("Code")
		columnCode.OptionsColumn.AllowEdit = False
		columnCode.Name = "Code"
		columnCode.FieldName = "Code"
		columnCode.Visible = False
		columnCode.Width = 50
		gvQualifikation.Columns.Add(columnCode)

		Dim columnJobName2Show As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobName2Show.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnJobName2Show.OptionsColumn.AllowEdit = False
		columnJobName2Show.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnJobName2Show.Name = "JobName2Show"
		columnJobName2Show.FieldName = "JobName2Show"
		columnJobName2Show.Visible = True
		columnJobName2Show.Width = 200 'BestFit()
		gvQualifikation.Columns.Add(columnJobName2Show)


		Dim completedColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		completedColumn.Caption = m_Translate.GetSafeTranslationValue("Meldepflicht?")
		completedColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		completedColumn.AppearanceHeader.Options.UseTextOptions = True
		completedColumn.Name = "MP"
		completedColumn.FieldName = "MP"
		completedColumn.Visible = m_LastSelectedDb = 4
		completedColumn.ColumnEdit = m_CheckEditCompleted
		completedColumn.Width = 50 'BestFit()
		gvQualifikation.Columns.Add(completedColumn)


		grdQualifikation.DataSource = Nothing

	End Sub

	Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)

		LinkLabel1.Text = m_Translate.GetSafeTranslationValue(LinkLabel1.Text)
		LinkLabel4.Text = m_Translate.GetSafeTranslationValue(LinkLabel4.Text)

		bsiNameInfo.Caption = m_Translate.GetSafeTranslationValue(bsiNameInfo.Caption)

		Me.cmdClose.Text = m_Translate.GetSafeTranslationValue(Me.cmdClose.Text)
		Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(Me.cmdOK.Text)

	End Sub


	Private Sub frmQualification_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iLastSelect = Me.m_LastSelectedDb
			My.Settings.iQualificationWidth = Me.Width
			My.Settings.iQualificationHight = Me.Height

			My.Settings.frmQualifikationLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.Save()
		End If

	End Sub

	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click

		'ClsDataDetail.GetReturnValue = String.Empty
		Me.Dispose()

	End Sub

	'Private Sub OngvQualifikation_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvQualifikation.CustomUnboundColumnData

	'	If e.Column.Name = "docType" Then
	'		If (e.IsGetData()) Then
	'			e.Value = CType(e.Row, JobData).MP
	'		End If
	'	End If
	'End Sub


	'Private Function LoadLocalQualificationData() As Boolean

	'	Dim listOfEmployees = GetLocalData4Qualifikation(Me._strGeschlecht)

	'	Dim responsiblePersonsGridData = (From person In listOfEmployees
	'									  Select New JobData With
	'	   {.Code = person.Code,
	'		.JobName2Show = person.JobName2Show,
	'		.Name_D_M = person.Name_D_M,
	'		.Name_D_W = person.Name_D_W,
	'		.Name_F_M = person.Name_F_M,
	'		.Name_I_M = person.Name_I_M,
	'		.Name_I_W = person.Name_I_W}).ToList()

	'	Dim listDataSource As BindingList(Of JobData) = New BindingList(Of JobData)

	'	For Each p In responsiblePersonsGridData
	'		listDataSource.Add(p)
	'	Next

	'	grdQualifikation.DataSource = listDataSource


	'	Return Not listOfEmployees Is Nothing
	'End Function


	Private Sub LinkLabel4_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
		Dim liLLTemplate As New List(Of String)
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim BarManagerContextMenu As New BarManager

		liLLTemplate.Add(m_Translate.GetSafeTranslationValue("4#CH-ISCO"))
		'liLLTemplate.Add(m_Translate.GetSafeTranslationValue("1#Seco Berufe"))
		liLLTemplate.Add(m_Translate.GetSafeTranslationValue("2#BGB-Berufliche Grundbildungen"))
		liLLTemplate.Add(m_Translate.GetSafeTranslationValue("3#HBB-Höhere Berufsbildungen"))

		If liLLTemplate.Count = 0 Then Return

		For i As Integer = 0 To liLLTemplate.Count - 1
			Dim myValue As String() = liLLTemplate(i).Split(CChar("#"))
			popupMenu.Manager = BarManagerContextMenu ' Me.BarManager1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			itm = New DevExpress.XtraBars.BarButtonItem
			itm.Caption = m_Translate.GetSafeTranslationValue(myValue(1).ToString)
			itm.AccessibleName = myValue(0).ToString

			popupMenu.AddItem(itm).BeginGroup = i = 0
			'popupMenu.AddItem(itm)
			AddHandler itm.ItemClick, AddressOf GetTemplateMnu

		Next
		popupMenu.ShowPopup(Cursor.Position) ' New Point(Me.LinkLabel4.Left, Me.LinkLabel4.Top + 10))

	End Sub

	Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
		'Dim ds As New DataSet
		'Dim dt As New DataTable

		Me.m_LastSelectedDb = 0
		ResetQualifikationGrid()
		LoadLocalQualificationData(Me.m_Geder, m_InitializationData.UserData.UserLanguage)

		'grdQualifikation.DataSource = dt
		'Me.grdQualifikation.Visible = True

		Me.bsiNameInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue(JOB_ART), m_Translate.GetSafeTranslationValue(If(Me.m_LastSelectedDb = 0, "Lokale", "Seco")))
		Me.bsiCountInfo.Caption = String.Format("{0}", Me.gvQualifikation.RowCount)

	End Sub

	Sub GetTemplateMnu(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		'Dim ds As New DataSet
		'Dim dt As New DataTable

		m_LastSelectedDb = e.Item.AccessibleName
		ResetQualifikationGrid()

		If e.Item.AccessibleName = "1" Then
			Me.m_LastSelectedDb = 1
			LoadSecoQualificationData(Me.m_Geder, m_InitializationData.UserData.UserLanguage)

		ElseIf e.Item.AccessibleName = "2" Then
			m_LastSelectedDb = 2
			LoadBGBQualificationData(m_Geder, m_InitializationData.UserData.UserLanguage)

		ElseIf e.Item.AccessibleName = "3" Then
			Me.m_LastSelectedDb = 3
			LoadHBBQualificationData(Me.m_Geder, m_InitializationData.UserData.UserLanguage)

		ElseIf e.Item.AccessibleName = "4" Then
			m_LastSelectedDb = 4
			LoadJobroomQualificationData(m_Geder, m_InitializationData.UserData.UserLanguage)

		End If

	End Sub

	Private Function LoadLocalQualificationData(ByVal gender As String, ByVal language As String) As Boolean
		Dim result As Boolean = True
		Dim jobJabel As String = String.Empty

		Dim data = m_TablesettingDatabaseAccess.LoadJobData()
		If data Is Nothing Then Return False

		Reset()

		Dim qualifcationData As New List(Of JobData)
		For Each itm In data
			Dim viewData As New JobData

			viewData.Code = itm.code.GetValueOrDefault(0)
			If gender.ToLower = "m" Then
				If language = "D" Then
					jobJabel = itm.beruf_d_m
				ElseIf language = "F" Then
					jobJabel = itm.beruf_f_m
				ElseIf language = "I" Then
					jobJabel = itm.beruf_i_m
				End If

			Else
				If language = "D" Then
					jobJabel = itm.beruf_d_w
				ElseIf language = "F" Then
					jobJabel = itm.beruf_f_w
				ElseIf language = "I" Then
					jobJabel = itm.beruf_i_w
				End If

			End If
			viewData.JobName2Show = jobJabel
			viewData.SelectedRec = False


			qualifcationData.Add(viewData)
		Next

		grdQualifikation.DataSource = qualifcationData
		bsiNameInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue(JOB_ART), m_Translate.GetSafeTranslationValue("Lokale"))
		Me.bsiCountInfo.Caption = String.Format("{0}", Me.gvQualifikation.RowCount)

		Return Not data Is Nothing
	End Function

	Private Function LoadSecoQualificationData(ByVal gender As String, ByVal language As String) As Boolean
		Dim result As Boolean = True

		Dim wsObject As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)

		Dim data = wsObject.PerformQualificationDataWebservice(gender, language, "seco")
		If data Is Nothing Then Return False

		Dim qualifcationData As New List(Of JobData)
		For Each itm In data
			Dim viewData As New JobData

			viewData.Code = itm.Code
			viewData.JobName2Show = itm.TranslatedValue
			viewData.SelectedRec = False

			qualifcationData.Add(viewData)
		Next

		grdQualifikation.DataSource = qualifcationData

		Me.bsiNameInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue(JOB_ART), m_Translate.GetSafeTranslationValue("Seco"))
		Me.bsiCountInfo.Caption = String.Format("{0}", Me.gvQualifikation.RowCount)

		Return Not data Is Nothing
	End Function

	Private Function LoadBGBQualificationData(ByVal gender As String, ByVal language As String) As Boolean
		Dim result As Boolean = True

		Dim wsObject As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)

		Dim data = wsObject.PerformQualificationDataWebservice(gender, language, "BGB")
		If data Is Nothing Then Return False

		Dim qualifcationData As New List(Of JobData)
		For Each itm In data
			Dim viewData As New JobData

			viewData.Code = itm.Code
			viewData.JobName2Show = itm.TranslatedValue
			viewData.SelectedRec = False

			qualifcationData.Add(viewData)
		Next

		grdQualifikation.DataSource = qualifcationData
		Me.bsiNameInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue(JOB_ART), m_Translate.GetSafeTranslationValue("BGB-Berufliche Grundbildungen"))
		Me.bsiCountInfo.Caption = String.Format("{0}", Me.gvQualifikation.RowCount)

		Return Not data Is Nothing
	End Function

	Private Function LoadHBBQualificationData(ByVal gender As String, ByVal language As String) As Boolean
		Dim result As Boolean = True

		Dim wsObject As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)

		Dim data = wsObject.PerformQualificationDataWebservice(gender, language, "HBB")
		If data Is Nothing Then Return False

		Dim qualifcationData As New List(Of JobData)
		For Each itm In data
			Dim viewData As New JobData

			viewData.Code = itm.Code
			viewData.JobName2Show = itm.TranslatedValue
			viewData.SelectedRec = False

			qualifcationData.Add(viewData)
		Next

		grdQualifikation.DataSource = qualifcationData
		Me.bsiNameInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue(JOB_ART), m_Translate.GetSafeTranslationValue("HBB-Höhere Berufsbildungen"))
		Me.bsiCountInfo.Caption = String.Format("{0}", Me.gvQualifikation.RowCount)

		Return Not data Is Nothing
	End Function

	Private Function LoadJobroomQualificationData(ByVal gender As String, ByVal language As String) As Boolean
		Dim result As Boolean = True

		Dim wsObject As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)

		Dim data = wsObject.PerformQualificationDataWebservice(gender, language, "jobroom")
		If data Is Nothing Then Return False

		Dim qualifcationData As New List(Of JobData)
		For Each itm In data
			Dim viewData As New JobData

			viewData.Code = itm.Code
			viewData.JobName2Show = itm.TranslatedValue
			viewData.MP = itm.MP
			viewData.SelectedRec = False

			qualifcationData.Add(viewData)
		Next

		grdQualifikation.DataSource = qualifcationData
		Me.bsiNameInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue(JOB_ART), m_Translate.GetSafeTranslationValue("CH-ISCO"))
		Me.bsiCountInfo.Caption = String.Format("{0}", Me.gvQualifikation.RowCount)

		Return Not data Is Nothing
	End Function

	Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
		Dim strValue As String = String.Empty

		Dim selectedData = If(SelectMultirecords, GetSelectedQualificatioinData(), SelectedRecord)
		If selectedData Is Nothing OrElse selectedData.Count = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten konnten ausgewählt werden."))

			Return
		End If

		Try
			For Each itm In selectedData
				strValue &= String.Format("{0}#{1}{2}", itm.Code, itm.JobName2Show, ITEM_SEPRATOR)

				If Me.m_LastSelectedDb > 0 Then AddRemoteDataToLocalTableData(New DataObjects.JobData With {.code = itm.Code, .beruf = itm.JobName2Show, .beruf_d_m = itm.JobName2Show})
			Next

			If strValue.EndsWith(ITEM_SEPRATOR) Then strValue = strValue.Substring(0, Len(strValue) - 1)
			GetSelectedData = strValue
			ClsDataDetail.GetReturnValue = strValue


			Me.Dispose()

		Catch ex As Exception
			Dim strMessage As String = "Fehler: {0}"
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue(strMessage), ex.ToString), m_Translate.GetSafeTranslationValue("Suche nach Qualifikationen"), MessageBoxIcon.Error)
			ClsDataDetail.GetReturnValue = String.Empty

		End Try

	End Sub

	Private Function AddRemoteDataToLocalTableData(ByVal job As SP.DatabaseAccess.TableSetting.DataObjects.JobData) As Boolean
		Dim result As Boolean = True
		Dim jobJabel As String = String.Empty

		result = result AndAlso m_TablesettingDatabaseAccess.AddJobData(job)
		If Not result Then
			m_Logger.LogWarning("Die neue Qualifikation konnte nicht hinzugefügt werden. Möglicherweise existiert bereits die Berufscode in lokaler Datenbank.")

			Return False
		End If

		Return result
	End Function


	'liSelectedValue2Transfer = New Dictionary(Of String, String)

	'Dim tbl As New DataTable()
	'If Me._iLastSelectedDb > 0 Then
	'	tbl.Columns.Add("Code", GetType(Integer))
	'	tbl.Columns.Add("Bez_D_M", GetType(String))
	'	tbl.Columns.Add("Bez_D_W", GetType(String))
	'	tbl.Columns.Add("Bez_F_M", GetType(String))
	'	tbl.Columns.Add("Bez_F_W", GetType(String))
	'	tbl.Columns.Add("Bez_I_M", GetType(String))
	'	tbl.Columns.Add("Bez_I_W", GetType(String))
	'	tbl.Columns.Add("Fach_D", GetType(String))
	'	tbl.Columns.Add("Fach_F", GetType(String))
	'	tbl.Columns.Add("Fach_I", GetType(String))
	'End If

	'grdSelectedrec.RefreshDataSource()
	'If Me._bAllowedMultiSelect Then
	'	Try
	'		For i As Integer = 0 To Me.gvSelectedrec.RowCount - 1
	'			Me.gvSelectedrec.FocusedRowHandle = i

	'			Dim dtr As DataRow
	'			dtr = gvSelectedrec.GetDataRow(i) 'GridView2.GetSelectedRows()(i))
	'			strValue = String.Format("{0}_{1}#{2}", If(Me._iLastSelectedDb = 0, "L", "R"),
	'															 dtr.Item("Code").ToString,
	'															 dtr.Item(strFieldName).ToString)

	'			liSelectedValue2Transfer.Add(String.Format("{0}", Val(dtr.Item("Code").ToString)),
	'																	 String.Format("{0}", dtr.Item(strFieldName).ToString))
	'			If Me._iLastSelectedDb > 0 Then
	'				tbl.Rows.Add(New Object() {String.Format("{0}", Val(dtr.Item("Code").ToString)),
	'																	 String.Format("{0}", dtr.Item("Bez_D_M").ToString),
	'																	 String.Format("{0}", dtr.Item("Bez_D_W").ToString),
	'																	 String.Format("{0}", dtr.Item("Bez_F_M").ToString),
	'																	 String.Format("{0}", dtr.Item("Bez_F_W").ToString),
	'																	 String.Format("{0}", dtr.Item("Bez_I_M").ToString),
	'																	 String.Format("{0}", dtr.Item("Bez_I_W").ToString),
	'																	 String.Format("{0}", dtr.Item("Fach_D").ToString),
	'																	 String.Format("{0}", dtr.Item("Fach_F").ToString),
	'																	 String.Format("{0}", dtr.Item("Fach_I").ToString)})
	'			End If

	'		Next

	'	Catch ex As Exception

	'	End Try

	'Else

	'	Try
	'		For i As Integer = 0 To gvQualifikation.SelectedRowsCount - 1
	'			Dim row As Integer = (gvQualifikation.GetSelectedRows()(i))
	'			If (gvQualifikation.GetSelectedRows()(i) >= 0) Then
	'				Dim dtr As DataRow
	'				dtr = gvQualifikation.GetDataRow(gvQualifikation.GetSelectedRows()(i))
	'				strValue = String.Format("{0}_{1}#{2}", If(Me._iLastSelectedDb = 0, "L", "R"),
	'																 dtr.Item("Code").ToString,
	'																 dtr.Item("Bezeichnung").ToString)

	'				liSelectedValue2Transfer.Add(String.Format("{0}", Val(dtr.Item("Code").ToString)),
	'																		 String.Format("{0}", dtr.Item("Bezeichnung").ToString))
	'				If Me._iLastSelectedDb > 0 Then
	'					tbl.Rows.Add(New Object() {String.Format("{0}", Val(dtr.Item("Code").ToString)),
	'																		 String.Format("{0}", dtr.Item("Bez_D_M").ToString),
	'																		 String.Format("{0}", dtr.Item("Bez_D_W").ToString),
	'																		 String.Format("{0}", dtr.Item("Bez_F_M").ToString),
	'																		 String.Format("{0}", dtr.Item("Bez_F_W").ToString),
	'																		 String.Format("{0}", dtr.Item("Bez_I_M").ToString),
	'																		 String.Format("{0}", dtr.Item("Bez_I_W").ToString),
	'																		 String.Format("{0}", dtr.Item("Fach_D").ToString),
	'																		 String.Format("{0}", dtr.Item("Fach_F").ToString),
	'																		 String.Format("{0}", dtr.Item("Fach_I").ToString)})
	'				End If
	'			End If
	'		Next i
	'	Catch ex As Exception

	'	End Try

	'End If

	'Try
	'	strValue = String.Empty
	'	For i As Integer = 0 To liSelectedValue2Transfer.Count - 1
	'		strValue &= String.Format("{0}#{1}{2}", liSelectedValue2Transfer.Keys(i), liSelectedValue2Transfer.Values(i), _strItemSeprator)
	'	Next

	'	If strValue.EndsWith(_strItemSeprator) Then strValue = strValue.Substring(0, Len(strValue) - 1)
	'	ClsDataDetail.GetReturnValue = strValue
	'	m_Logger.LogDebug(strValue)

	'	If Me._iLastSelectedDb > 0 Then SaveJobsIntoLocalDb(tbl)
	'	Me.Dispose()

	'Catch ex As Exception
	'	Dim strMessage As String = "Fehler: {0}"
	'	DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue(strMessage), ex.Message),
	'																						 m_Translate.GetSafeTranslationValue("Suche nach Qualifikationen"),
	'																						 System.Windows.Forms.MessageBoxButtons.OK,
	'																						 System.Windows.Forms.MessageBoxIcon.Error)
	'	ClsDataDetail.GetReturnValue = String.Empty

	'End Try

	'End Sub


	'Private Sub grdQualifikation_DataSourceChanged(sender As Object, e As System.EventArgs) Handles grdQualifikation.DataSourceChanged
	'	Dim i As Integer = 0

	'	For Each col As GridColumn In Me.gvQualifikation.Columns
	'		col.MinWidth = 0
	'		Try
	'			col.Visible = i = 1
	'			col.Caption = m_Translate.GetSafeTranslationValue(col.GetCaption)

	'		Catch ex As Exception
	'			col.Visible = False

	'		End Try
	'		i += 1
	'	Next col

	'End Sub

	'Private Sub grdQualifikation_DoubleClick(sender As Object, e As System.EventArgs) Handles grdQualifikation.DoubleClick
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strValue As String = String.Empty
	'	Dim strTbleName As String = String.Empty
	'	Dim bInsertRow As Boolean = True
	'	liSelectedValue2Transfer = New Dictionary(Of String, String)

	'	Dim tbl As New DataTable()
	'	tbl.Columns.Add("Code", GetType(Integer))
	'	tbl.Columns.Add("Bez_D_M", GetType(String))
	'	tbl.Columns.Add("Bez_D_W", GetType(String))
	'	tbl.Columns.Add("Bez_F_M", GetType(String))
	'	tbl.Columns.Add("Bez_F_W", GetType(String))
	'	tbl.Columns.Add("Bez_I_M", GetType(String))
	'	tbl.Columns.Add("Bez_I_W", GetType(String))
	'	tbl.Columns.Add("Fach_D", GetType(String))
	'	tbl.Columns.Add("Fach_F", GetType(String))
	'	tbl.Columns.Add("Fach_I", GetType(String))

	'	Try
	'		For i As Integer = 0 To gvQualifikation.SelectedRowsCount - 1
	'			Dim row As Integer = (gvQualifikation.GetSelectedRows()(i))
	'			If (gvQualifikation.GetSelectedRows()(i) >= 0) Then

	'				dtr_Copy = gvQualifikation.GetDataRow(gvQualifikation.GetSelectedRows()(i))
	'				strValue = String.Format("{0}_{1}#{2}", If(Me._iLastSelectedDb = 0, "L", "R"),
	'																 dtr_Copy.Item("Code").ToString,
	'																 dtr_Copy.Item("Bezeichnung").ToString)

	'				liSelectedValue2Transfer.Add(String.Format("{0}", Val(dtr_Copy.Item("Code").ToString)),
	'																		 String.Format("{0}", dtr_Copy.Item("Bezeichnung").ToString))

	'				tbl.Rows.Add(New Object() {String.Format("{0}", Val(dtr_Copy.Item("Code").ToString)),
	'																		 String.Format("{0}", dtr_Copy.Item("Bez_D_M").ToString),
	'																		 String.Format("{0}", dtr_Copy.Item("Bez_D_W").ToString),
	'																		 String.Format("{0}", dtr_Copy.Item("Bez_F_M").ToString),
	'																		 String.Format("{0}", dtr_Copy.Item("Bez_F_W").ToString),
	'																		 String.Format("{0}", dtr_Copy.Item("Bez_I_M").ToString),
	'																		 String.Format("{0}", dtr_Copy.Item("Bez_I_W").ToString),
	'																		 String.Format("{0}", dtr_Copy.Item("Fach_D").ToString),
	'																		 String.Format("{0}", dtr_Copy.Item("Fach_F").ToString),
	'																		 String.Format("{0}", dtr_Copy.Item("Fach_I").ToString)})

	'				bInsertRow = Not IsRowInListing(dtr_Copy.Item("Code").ToString)
	'				If bInsertRow Then Me.gvSelectedrec.AddNewRow()

	'				grdSelectedrec.RefreshDataSource()

	'			End If
	'		Next i
	'		strValue = String.Empty
	'		If Me.grdSelectedrec.DataSource Is Nothing Then Me.grdSelectedrec.DataSource = tbl
	'		Me.grdSelectedrec.Refresh()


	'	Catch ex As Exception
	'		Dim strMessage As String = "Fehler: {0}"
	'		DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue(strMessage), ex.Message),
	'																							 m_Translate.GetSafeTranslationValue("Suche nach Qualifikationen"),
	'																							 System.Windows.Forms.MessageBoxButtons.OK,
	'																							 System.Windows.Forms.MessageBoxIcon.Error)
	'		ClsDataDetail.GetReturnValue = String.Empty

	'	End Try

	'End Sub

	'Function IsRowInListing(ByVal strSelectedText As String) As Boolean
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bResult As Boolean = False

	'	For i As Integer = 0 To Me.gvSelectedrec.RowCount
	'		Me.gvSelectedrec.FocusedRowHandle = i
	'		If Me.gvSelectedrec.GetFocusedRowCellValue("Code") = strSelectedText Then
	'			Return True
	'		End If
	'	Next

	'	Return bResult
	'End Function

	'Private Sub grdSelectedrec_DataSourceChanged(sender As Object, e As System.EventArgs) Handles grdSelectedrec.DataSourceChanged
	'	Dim i As Integer = 0
	'	Dim strUSLanguage As String = m_InitializationData.UserData.UserLanguage
	'	Dim fieldName As String = String.Format("Bez_{0}_{1}", strUSLanguage, Me._strGeschlecht)


	'	For Each col As GridColumn In Me.gvSelectedrec.Columns
	'		'Trace.WriteLine(col.FieldName)
	'		col.MinWidth = 0
	'		Try

	'			If (col.FieldName.ToLower) = fieldName.ToLower Then
	'				col.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
	'				col.Visible = True
	'			Else
	'				col.Visible = False
	'			End If
	'			col.Caption = m_Translate.GetSafeTranslationValue(col.GetCaption)

	'		Catch ex As Exception
	'			col.Visible = False

	'		End Try
	'		i += 1
	'	Next col

	'End Sub

	'Private Sub grdSelectedrec_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Code", Val(dtr_Copy.Item("Code")))
	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Bez_D_M", dtr_Copy.Item("Bez_D_M"))
	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Bez_D_W", dtr_Copy.Item("Bez_D_W"))
	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Bez_F_M", dtr_Copy.Item("Bez_F_M"))
	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Bez_F_W", dtr_Copy.Item("Bez_F_W"))
	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Bez_I_M", dtr_Copy.Item("Bez_I_M"))
	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Bez_I_W", dtr_Copy.Item("Bez_I_W"))

	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Fach_D", dtr_Copy.Item("Fach_D"))
	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Fach_F", dtr_Copy.Item("Fach_F"))
	'	Me.gvSelectedrec.SetRowCellValue(e.RowHandle, "Fach_I", dtr_Copy.Item("Fach_I"))

	'	Me.gvSelectedrec.UpdateCurrentRow()
	'End Sub

	'Private Sub grdSelectedrec_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles grdSelectedrec.KeyDown

	'	If (e.KeyCode = Keys.Delete) Then ' And e.Modifiers = Keys.Control) Then
	'		Me.gvSelectedrec.DeleteRow(Me.gvSelectedrec.FocusedRowHandle)
	'	End If

	'End Sub

	Private Sub frmQualification_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

		'Try
		'	Me.KeyPreview = True
		'	Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		'	If strStyleName <> String.Empty Then
		'		UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		'	End If

		'Catch ex As Exception
		'	m_Logger.LogError(ex.ToString())

		'	End Try


		'Try
		'	Dim UpdateDelegate As New MethodInvoker(AddressOf TranslateControls)
		'	Me.Invoke(UpdateDelegate)

		'Catch ex As Exception
		'	m_Logger.LogError(String.Format("{0}.Formtranslation: {1}", strMethodeName, ex.Message))

		'End Try

		'Me.grdSelectedrec.Visible = Me._bAllowedMultiSelect
		'If _bAllowedMultiSelect Then grdQualifikation.Width = grdSelectedrec.Width
		'If Not _bAllowedMultiSelect Then grdQualifikation.Anchor += AnchorStyles.Right 'Me.MaximumSize = Me.Size



		'If Me._iLastSelectedDb = 0 Then
		'	'Dim data = GetLocalData4Qualifikation(Me._strGeschlecht)
		'	'grdQualifikation.DataSource = data

		'	dt = GetLocalData4Qualifikation_(Me._strGeschlecht)
		'	grdQualifikation.DataSource = dt

	End Sub


	'Private Sub gvMainOnDoubleClick(sender As Object, e As System.EventArgs)
	'	Dim strValue As String = String.Empty
	'	Dim view As GridView = TryCast(sender, GridView)

	'	Try
	'		Dim selectedrow = SelectedRecord

	'		If Not selectedrow Is Nothing Then



	'		End If



	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)
	'		m_UtilityUI.ShowErrorDialog(ex.Message)

	'	End Try

	'End Sub


	Private Class JobData

		Public Property Code As Integer
		Public Property JobName2Show As String

		Public Property Name_D_M As String
		Public Property Name_D_W As String

		Public Property Name_F_M As String
		Public Property Name_F_W As String

		Public Property Name_I_M As String
		Public Property Name_I_W As String

		Public Property Fach_D As String
		Public Property Fach_I As String
		Public Property Fach_F As String
		Public Property SelectedRec As Boolean
		Public Property MP As Boolean

	End Class


End Class