
Imports System.IO

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.XtraGrid.Columns
Imports System.Windows.Forms

'Imports DevComponents.DotNetBar

Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath.ClsProgPath
Imports DevExpress.LookAndFeel
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports System.ComponentModel

Public Class frmBranches

#Region "private consts"

	Private Const JOB_ART As String = "Branchendatenbank"
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

	Private liSelectedValue2Transfer As Dictionary(Of String, String)
	Private m_LastSelectedDb As Short = 0
	Private _bAllowedMultiSelect As Boolean = False
	Private _strItemSeprator As String = "|"

	Private MouseX As Integer
	Private MouseY As Integer

	Private m_Mandant As Mandant
	Protected m_UtilityUI As UtilityUI

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

		TranslateControls()
		Reset()

	End Sub

#End Region


#Region "Public Properties"

	Public Property GetSelectedData As String
	Public Property SelectMultirecords As Boolean

#End Region


#Region "Private Properties"

	Private ReadOnly Property SelectedRecord As BindingList(Of SectorViewData)
		Get
			Dim Result = New BindingList(Of SectorViewData)

			Try

				Dim gvRP = TryCast(grdBranches.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvRP Is Nothing) Then

					Dim selectedRows = gvRP.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim job = CType(gvRP.GetRow(selectedRows(0)), SectorViewData)

						Result.Add(job)
						Return Result
					End If

				End If

			Catch ex As Exception

			End Try

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property GetSelectedQualificatioinData() As BindingList(Of SectorViewData)
		Get

			Dim result As BindingList(Of SectorViewData) = Nothing
			Try
				grdBranches.RefreshDataSource()
				Dim printList = CType(grdBranches.DataSource, BindingList(Of SectorViewData))
				Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

				result = New BindingList(Of SectorViewData)

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


#Region "public Methodes"

	Public Function LoadBranchesData() As Boolean
		Dim result As Boolean = True

		LoadLocalBranchesData(m_InitializationData.UserData.UserLanguage)

		Return result
	End Function

#End Region

#Region "form methodes"

	Private Sub frmBranches_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iBranchesWidth = Me.Width
			My.Settings.iBranchesHeight = Me.Height

			My.Settings.frmBranchesLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.Save()
		End If

	End Sub

	Private Sub frmBranches_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		'Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'Dim dt As New DataTable
		m_LastSelectedDb = My.Settings.iLastSelect

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

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

		Catch ex As Exception
			m_Logger.LogError(String.Format("FormStyle: {0}", ex.ToString))

		End Try

		'dt = GetDbData4Branche()
		'_bAllowedMultiSelect = ClsDataDetail.AllowedMultiSelect
		'Me.gvQualifikation.OptionsSelection.MultiSelect = _bAllowedMultiSelect

		'grdQualifikation.DataSource = dt
		'Me.gvQualifikation.ShowFindPanel()
		'Dim strRecAnz As String = "Branchendatenbank: {0}"
		'Me.bsiNameInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue(strRecAnz), Me.gvQualifikation.RowCount)

	End Sub

	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click

		ClsDataDetail.GetReturnValue = String.Empty
		Me.Dispose()

	End Sub


#End Region




	Private Sub Reset()
		ResetBranchesGrid()
	End Sub

	Private Sub ResetBranchesGrid()

		gvBranches.OptionsView.ShowIndicator = False
		gvBranches.OptionsView.ShowAutoFilterRow = True
		gvBranches.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvBranches.OptionsView.ShowFooter = False
		gvBranches.OptionsBehavior.Editable = True
		gvBranches.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvBranches.Columns.Clear()

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
			gvBranches.Columns.Add(columnSelectedRec)
		End If

		Dim columnCode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCode.Caption = m_Translate.GetSafeTranslationValue("Code")
		columnCode.OptionsColumn.AllowEdit = False
		columnCode.Name = "Code"
		columnCode.FieldName = "Code"
		columnCode.Visible = False
		columnCode.Width = 50
		gvBranches.Columns.Add(columnCode)

		Dim columnTranslatedValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTranslatedValue.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTranslatedValue.OptionsColumn.AllowEdit = False
		columnTranslatedValue.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnTranslatedValue.Name = "TranslatedValue"
		columnTranslatedValue.FieldName = "TranslatedValue"
		columnTranslatedValue.Visible = True
		columnTranslatedValue.Width = 200 'BestFit()
		gvBranches.Columns.Add(columnTranslatedValue)


		grdBranches.DataSource = Nothing

	End Sub

	Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)

		bsiNameInfo.Caption = m_Translate.GetSafeTranslationValue(bsiNameInfo.Caption)

		Me.cmdClose.Text = m_Translate.GetSafeTranslationValue(Me.cmdClose.Text)
		Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(Me.cmdOK.Text)

	End Sub


	Private Function LoadLocalBranchesData(ByVal language As String) As Boolean
		Dim result As Boolean = True
		Dim sectorLabel As String = String.Empty

		Dim data = m_TablesettingDatabaseAccess.LoadSectorData()
		If data Is Nothing Then Return False

		Reset()

		Dim qualifcationData As New List(Of SectorViewData)
		Dim listDataSource As BindingList(Of SectorViewData) = New BindingList(Of SectorViewData)

		For Each itm In data

			If language = "F" Then
				sectorLabel = itm.bez_f
			ElseIf language = "I" Then
				sectorLabel = itm.bez_i
			ElseIf language = "E" Then
				sectorLabel = itm.bez_e

			Else
				sectorLabel = itm.bez_d
			End If

			Dim invoiceAddressViewData = New SectorViewData() With {
								.code = itm.code,
								.bez_d = itm.bez_d,
								.bez_e = itm.bez_e,
								.bez_f = itm.bez_f,
								.bez_i = itm.bez_i,
								.branche = itm.branche,
								.TranslatedValue = sectorLabel,
								.SelectedRec = False}

			listDataSource.Add(invoiceAddressViewData)
		Next

		grdBranches.DataSource = listDataSource
		bsiNameInfo.Caption = m_Translate.GetSafeTranslationValue(JOB_ART)
		Me.bsiCountInfo.Caption = String.Format("{0}", Me.gvBranches.RowCount)

		Return Not data Is Nothing
	End Function



	Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
		Dim strValue As String = String.Empty

		Dim selectedData = If(SelectMultirecords, GetSelectedQualificatioinData(), SelectedRecord)
		If selectedData Is Nothing OrElse selectedData.Count = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten konnten ausgewählt werden."))

			Return
		End If

		Try
			For Each itm In selectedData
				strValue &= String.Format("{0}#{1}{2}", itm.code, itm.TranslatedValue, ITEM_SEPRATOR)
			Next

			If strValue.EndsWith(ITEM_SEPRATOR) Then strValue = strValue.Substring(0, Len(strValue) - 1)
			GetSelectedData = strValue
			
			Me.Dispose()

		Catch ex As Exception
			Dim strMessage As String = "Fehler: {0}"
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue(strMessage), ex.ToString), m_Translate.GetSafeTranslationValue("Suche nach Branchen"), MessageBoxIcon.Error)
			ClsDataDetail.GetReturnValue = String.Empty

		End Try

	End Sub


	'Dim strValue As String = String.Empty
	'Dim strTbleName As String = String.Empty
	'liSelectedValue2Transfer = New Dictionary(Of String, String)

	'Try
	'	For i As Integer = 0 To gvQualifikation.SelectedRowsCount - 1
	'		Dim row As Integer = (gvQualifikation.GetSelectedRows()(i))
	'		If (gvQualifikation.GetSelectedRows()(i) >= 0) Then
	'			Dim dtr As DataRow
	'			dtr = gvQualifikation.GetDataRow(gvQualifikation.GetSelectedRows()(i))
	'			strValue = String.Format("{0}#{1}", dtr.Item("Code").ToString,
	'									 dtr.Item("Bezeichnung").ToString)

	'			liSelectedValue2Transfer.Add(String.Format("{0}", dtr.Item("Code").ToString),
	'										 String.Format("{0}", dtr.Item("Bezeichnung").ToString))
	'		End If
	'	Next i
	'	strValue = String.Empty

	'	For i As Integer = 0 To liSelectedValue2Transfer.Count - 1
	'		strValue &= String.Format("{0}#{1}{2}", liSelectedValue2Transfer.Keys(i), liSelectedValue2Transfer.Values(i),
	'								  _strItemSeprator)
	'	Next

	'	If strValue.EndsWith(_strItemSeprator) Then strValue = strValue.Substring(0, Len(strValue) - 1)
	'	ClsDataDetail.GetReturnValue = strValue

	'	Me.Dispose()

	'Catch ex As Exception
	'	Dim strMessage As String = "Fehler: {0}"
	'	DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue(strMessage), ex.Message),
	'																						 m_Translate.GetSafeTranslationValue("Suche nach Branchen"),
	'																						 System.Windows.Forms.MessageBoxButtons.OK,
	'																						 System.Windows.Forms.MessageBoxIcon.Error)
	'	ClsDataDetail.GetReturnValue = String.Empty

	'End Try


	'Private Sub grdQualifikation_DataSourceChanged(sender As Object, e As System.EventArgs) Handles grdQualifikation.DataSourceChanged
	'	Dim i As Integer = 0

	'	For Each col As GridColumn In Me.gvQualifikation.Columns
	'		'Trace.WriteLine(col.FieldName)
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

	Private Class SectorViewData
		Inherits SectorData
		Public Property TranslatedValue As String
		Public Property SelectedRec As Boolean

	End Class

End Class