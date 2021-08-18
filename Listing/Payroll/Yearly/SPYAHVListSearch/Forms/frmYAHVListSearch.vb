
Option Strict Off

Imports System.Reflection.Assembly

Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common.DataObjects

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports System.IO

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports SPProgUtility.ColorUtility.ClsColorUtility

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid
Imports SP.DatabaseAccess.Common
Imports SPS.Listing.Print.Utility


Public Class frmYAHVListSearch
	Inherits XtraForm

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Private Property CommonDbAccess As ICommonDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private Shared m_Logger As ILogger = New Logger()
	Private connectionString As String

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Public Shared frmMyLV As frmListeSearch_LV

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private _abortDeniedPending As Boolean

	Private m_SearchCriteria As New SearchCriteria

#End Region


#Region "Private Constants"

	Private Const frmMyLVName As String = "frmListeSearch_LV"
	Private Const strValueSeprator As String = "#@"

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

		m_mandant = New Mandant
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		TranslateControls()

		Reset()
		LoadMandantenDropDown()

		AddHandler Cbo_Kanton.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_Filiale.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_Nationality.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub


#End Region


#Region "Lookup Edit Reset und Load..."

	Private Sub Reset()

		ResetMandantenDropDown()

	End Sub

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

	End Sub

	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData) ' MandantenData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(CInt(lueMandant.EditValue), ClsDataDetail.m_InitialData.UserData.UserNr)

			m_InitializationData = ClsDataDetail.m_InitialData

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)

	End Sub


#End Region


	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property

	Private ReadOnly Property GetJobID(ByVal Art As Integer) As String
		Get
			Return If(Art = 0, "9.8", "9.9")
		End Get
	End Property



#Region "Private Methods"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblkanton.Text = m_Translate.GetSafeTranslationValue(Me.lblkanton.Text)
		Me.lblFiliale.Text = m_Translate.GetSafeTranslationValue(Me.lblFiliale.Text)
		Me.lblNationality.Text = m_Translate.GetSafeTranslationValue(Me.lblNationality.Text)
		lblFilialWarning.Text = m_Translate.GetSafeTranslationValue(lblFilialWarning.Text)

		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblSQLAbfrage.Text)

		Me.beiWorking.Caption = m_Translate.GetSafeTranslationValue(Me.beiWorking.Caption)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClearFields.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClearFields.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

	End Sub


#End Region





#Region "Dropdown Funktionen Allgemein"


	Private Sub ListLOJahr_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_JahrVon.QueryPopUp
		ListLOJahr(sender)
	End Sub

	Private Sub Cbo_MAGeschaeftsstellen_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		Dim jahr As Integer = Now.Year
		If Cbo_JahrVon.EditValue Is Nothing Then Cbo_JahrVon.EditValue = jahr Else jahr = Val(Cbo_JahrVon.EditValue)

		ListEmployeeFiliale(Cbo_Filiale, jahr)

	End Sub

	Private Sub Cbo_Kanton_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Kanton.QueryPopUp
		ListMAKanton(Me.Cbo_Kanton, Me.Cbo_JahrVon.EditValue)
	End Sub

	Private Sub Cbo_Nationality_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Nationality.QueryPopUp
		ListMANationality(Cbo_Nationality, Me.Cbo_JahrVon.EditValue)
	End Sub

#End Region


	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub OnFrmDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded(frmMyLVName, True)

		Try
			My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.ifrmHeight = Me.Height
			My.Settings.ifrmWidth = Me.Width

			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub OnFrmMove(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		Try
			If FormIsLoaded(frmMyLVName, False) Then
				frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
				frmMyLV.BringToFront()
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub OnFrmLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try

			TranslateControls()
			SetInitialFields()

			Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		Catch ex As Exception

		End Try


	End Sub

	Private Sub SetInitialFields()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try
		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
			If My.Settings.frm_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = "0"
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.Message))

		End Try

		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
			Me.lueMandant.Visible = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 675, m_InitializationData.MDData.MDNr)
			Me.lblMDName.Visible = Me.lueMandant.Visible

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

			If m_InitializationData.UserData.UserNr <> 1 Then Me.XtraTabControl1.TabPages.Remove(Me.xtabSQLQuery)

			bbiClearFields.Enabled = True
			bbiPrint.Enabled = False
			bbiExport.Enabled = False

			FillDefaultValues()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Mandantenauswahl anzeigen: {0}", ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

	End Sub

	Private Sub OnFrmKeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine, _
														 GetExecutingAssembly().FullName, _
														 GetExecutingAssembly().Location, _
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

	''' <summary>
	''' Default-Werte in den Felder Monat und Jahr einfügen.
	''' </summary>
	''' <remarks></remarks>
	Sub FillDefaultValues()

		FillDefaultDates()

	End Sub

	Private Sub FillDefaultDates()
		Try

			If Me.Cbo_JahrVon.Properties.Items.Count = 0 Then
				ListLOJahr(Me.Cbo_JahrVon)
			End If

			Me.Cbo_JahrVon.EditValue = Now.Year

		Catch ex As Exception

		End Try

	End Sub


	' ''' <summary>
	' ''' Daten fürs Drucken bereit stellen.
	' ''' </summary>
	' ''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	' ''' <param name="bForExport">ob die Liste für Export ist.</param>
	' ''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	' ''' <remarks></remarks>
	' ''' 
	'Sub GetData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
	'  Dim iKDNr As Integer = 0
	'  Dim iKDZNr As Integer = 0
	'  Dim bResult As Boolean = True
	'  Dim bWithKD As Boolean = True

	'  Dim sSql As String = ClsDataDetail.GetSQLQuery()
	'  If sSql = String.Empty Then
	'    MsgBox(TranslateMyText("Keine Suche wurde gestartet!"), _
	'           MsgBoxStyle.Exclamation, "GetData4Print_0")
	'    Exit Sub
	'  End If

	'  Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
	'  Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

	'  Try
	'    Conn.Open()

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader
	'    Try
	'      If Not rZGrec.HasRows Then
	'        cmd.Dispose()
	'        rZGrec.Close()

	'        MessageBox.Show(TranslateMyText("Ich konnte leider Keine Daten finden."), _
	'                        "GetData4Print_1", MessageBoxButtons.OK, MessageBoxIcon.Information)
	'        Exit Sub
	'      End If

	'    Catch ex As Exception
	'      MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetData4Print_2")
	'      Exit Sub

	'    End Try

	'    While rZGrec.Read

	'      'If LL.Core.Handle <> 0 Then LL.Core.LlJobClose()
	'      'LL.Core.LlJobOpen(2)

	'      If bForExport Then
	'        '            ExportLLDoc(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex),CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
	'        '          bResult = ExportLLDocWithStorage(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex), CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
	'      Else
	'        If bForDesign Then
	'          ShowInDesign(LL, strJobInfo, rZGrec)

	'        Else
	'          bResult = ShowInPrint(LL, strJobInfo, rZGrec)

	'        End If
	'      End If

	'      If Not bResult Or bForDesign Then Exit While
	'    End While
	'    rZGrec.Close()

	'  Catch ex As Exception
	'    MsgBox(ex.Message, MsgBoxStyle.Critical, "GetData4Print_3")

	'  Finally
	'    cmd.Dispose()
	'    Conn.Close()

	'  End Try

	'End Sub


	'Private Sub mnuDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDesign.Click

	'  GetData4Print(True, False, ClsDataDetail.GetModulToPrint())

	'End Sub

	'Private Sub mnuListPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuListPrint.Click, mnuListPrint_1.Click
	'  Dim _ClsDb As New ClsDbFunc

	'  If sender.ToString.Contains("freie") Then
	'    _ClsDb.GetJobNr4Print(Me, 1)

	'  Else
	'    _ClsDb.GetJobNr4Print(Me, 0)

	'  End If
	'  GetData4Print(False, False, ClsDataDetail.GetModulToPrint())

	'End Sub


#Region "Funktionen zur Menüaufbau..."


#Region "zum Leeren..."

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClearFields_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClearFields.ItemClick

		FormIsLoaded("frmListeSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()
		FillDefaultValues()

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiSearch.Enabled = True
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		Me.txt_SQL_1.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")
		Me.txt_SQL_2.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		Try
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item(Me.XtraTabControl1.Name).Controls
				For Each con As Control In tabPg.Controls
					ResetControl(con)
				Next
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Funktion ruft sich rekursiv auf.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		Try
			If con.Name.ToLower = lueMandant.Name.ToLower Then Exit Sub

			' Rekursiver Aufruf
			' Sonst Control zurücksetzen
			If TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
				Dim tb As DevExpress.XtraEditors.TextEdit = CType(con, DevExpress.XtraEditors.TextEdit)
				tb.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = CType(con, DevExpress.XtraEditors.ComboBoxEdit)
				cbo.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = CType(con, DevExpress.XtraEditors.CheckedComboBoxEdit)
				cbo.EditValue = Nothing

			ElseIf con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next
			Else

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

#End Region


	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick


		Try
			bbiClearFields.Enabled = False
			bbiPrint.Enabled = False
			bbiExport.Enabled = False

			FormIsLoaded(frmMyLVName, True)

			m_SearchCriteria = GetSearchKrieteria()
			If Not (Kontrolle()) Then Exit Sub
			Me.txt_SQL_1.Text = String.Empty
			Me.txt_SQL_2.Text = String.Empty


			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			GetMyQueryString()


		Catch ex As Exception

		Finally

		End Try

	End Sub

	Function Kontrolle() As Boolean
		Try
			Dim msg As String = ""

			Dim jahrVon As Integer? = m_SearchCriteria.FirstYear

			If lueMandant.EditValue Is Nothing Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben keinen Mandanten ausgewählt.{0}"), vbNewLine)
			End If

			If Not jahrVon.HasValue Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben Jahr ausgewählt.{0}"), vbNewLine)
			End If

			If Not String.IsNullOrWhiteSpace(msg) Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien unvollständig:{0}{1}"), vbNewLine, msg)
				m_UtilityUI.ShowErrorDialog(msg)

				Return False
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

		Return True
	End Function

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria


		result.listname = m_Translate.GetSafeTranslationValue("AHV-Lohnbescheinigung")
		result.mandantenname = lueMandant.Text

		If Me.Cbo_JahrVon.EditValue Is Nothing Then Me.Cbo_JahrVon.EditValue = Now.Year

		result.FirstYear = Cbo_JahrVon.EditValue
		result.filiale = Cbo_Filiale.EditValue
		result.kanton = Me.Cbo_Kanton.EditValue
		result.nationality = Me.Cbo_Nationality.EditValue

		m_SearchCriteria.sqlsearchstring = String.Empty

		Return result

	End Function

	Function GetMyQueryString() As Boolean


		BackgroundWorker1.WorkerSupportsCancellation = True
		BackgroundWorker1.WorkerReportsProgress = True
		BackgroundWorker1.RunWorkerAsync()


		'Dim _ClsDb As New ClsDbFunc

		'If ClsDataDetail.GetSQLQuery() = String.Empty Then
		'	_ClsDb.GetJobNr4Print(Me, 0)


		'	sSql1Query = _ClsDb.GetStartSQLString(Me)		 ' 1. String
		'	sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, Me)		' Where Klausel

		'	If Trim(sSql2Query) <> String.Empty Then sSql1Query += " Where "

		'	strSort = _ClsDb.GetSortString(Me)			' Sort Klausel
		'	ClsDataDetail.GetSQLSortString = strSort
		'	ClsDataDetail.GetSQLQuery = sSql1Query & " " & sSql2Query & " " & strSort

		'	Me.txt_SQL_1.Text = ClsDataDetail.GetSQLQuery
		'	' Die 1. Suchphase in der LOL-Datenbank wurde abgeschlossen. Nun müssen die Daten zusammengestellt werden.

		'	BackgroundWorker1.WorkerSupportsCancellation = True
		'	BackgroundWorker1.RunWorkerAsync()		' Multithreading starten
		'End If

		Return True
	End Function

#End Region




#Region "Multitreading..."

	Private Sub OnBgWorkerDoWork(ByVal sender As System.Object, _
																		 ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		CheckForIllegalCrossThreadCalls = False
		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		Dim i As Integer = 0
		Dim cForeColor As New System.Drawing.Color
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

		Me.bbiSearch.Enabled = False

		Try
			Dim sSqlQuerySelect As String = String.Empty

			Dim _ClsDb As New ClsDbFunc(m_SearchCriteria)
			Dim sSql1Query As String = _ClsDb.GetStartSQLString()
			If String.IsNullOrWhiteSpace(sSql1Query) Then Throw New Exception("Fehler beim Löschen der Daten.")
			Dim sSql2Query As String = _ClsDb.GetQuerySQLString()		' Where Klausel

			If Not String.IsNullOrWhiteSpace(sSql2Query) Then sSql1Query += " Where "

			Dim strSort As String = _ClsDb.GetSortString()
			ClsDataDetail.GetSQLSortString = strSort
			ClsDataDetail.GetSQLQuery = sSql1Query & " " & sSql2Query & " " & strSort
			m_Logger.LogDebug(String.Format("Query_1: {0}", ClsDataDetail.GetSQLQuery))

			Me.txt_SQL_1.Text = ClsDataDetail.GetSQLQuery

			_ClsDb.BuildYAHVDb(ClsDataDetail.GetSQLQuery, m_SearchCriteria.filiale, m_SearchCriteria.FirstYear)
			sSql1Query = _ClsDb.GetStartSQLString_2()		 ' 2. String für Drucken (die Whereklausel kommt nicht mehr.
			'strSort = _ClsDb.GetSortString_2(Me)           ' Sort Klausel
			ClsDataDetail.GetSQLSortString_2 = strSort

			ClsDataDetail.GetSQLQuery() = sSql1Query '+ ClsDataDetail.GetSQLSortString_2()
			txt_SQL_2.Text = ClsDataDetail.GetSQLQuery()
			m_Logger.LogDebug(String.Format("Query_2: {0}", ClsDataDetail.GetSQLQuery))


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		Finally
			e.Result = True
			If bw.CancellationPending Then e.Cancel = True
			Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		End Try

	End Sub

	Private Sub OnBgWorkerProgressChanged(ByVal sender As Object, _
																								ByVal e As System.ComponentModel.ProgressChangedEventArgs) _
																								Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub OnBgWorkerRunWorkerCompleted(ByVal sender As Object, _
																									 ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) _
																									 Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			m_UtilityUI.ShowErrorDialog(e.ToString)
		Else
			If e.Cancelled = True Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Vorgang wurde abgebrochen."))

			Else
				BackgroundWorker1.CancelAsync()

				' Daten auflisten...
				If Not FormIsLoaded(frmMyLVName, True) Then
					frmMyLV = New frmListeSearch_LV(Me.txt_SQL_2.Text, String.Empty)
					frmMyLV.Show()
					frmMyLV.BringToFront()
				End If

				Dim reccount As Integer = frmMyLV.RecCount
				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...", _
																					 reccount)
				frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...", _
																								reccount)

				' Die Buttons Drucken und Export aktivieren
				Me.bbiPrint.Enabled = reccount > 0
				Me.bbiExport.Enabled = reccount > 0
				If reccount > 0 Then
					CreatePrintPopupMenu()
					CreateExportPopupMenu()
				End If

			End If
		End If
		bbiClearFields.Enabled = True
		Me.bbiSearch.Enabled = True
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

	End Sub


	'Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
	'	Dim _ClsDb As New ClsDbFunc
	'	Dim sSql1Query As String = String.Empty
	'	Dim sSql2Query As String = String.Empty
	'	Dim strSort As String = String.Empty

	'	CheckForIllegalCrossThreadCalls = False

	'	Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

	'	_ClsDb.BuildYAHVDb(ClsDataDetail.GetSQLQuery, Me.Cbo_Filiale.Text, Me.Cbo_Year.Text)
	'	sSql1Query = _ClsDb.GetStartSQLString_2(Me)		 ' 2. String für Drucken (die Whereklausel kommt nicht mehr.
	'	'strSort = _ClsDb.GetSortString_2(Me)           ' Sort Klausel
	'	ClsDataDetail.GetSQLSortString_2 = strSort

	'	ClsDataDetail.GetSQLQuery() = sSql1Query + ClsDataDetail.GetSQLSortString_2()

	'	e.Result = True
	'	If bw.CancellationPending Then e.Cancel = True

	'End Sub

	'Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
	'	Trace.WriteLine(e.ToString)
	'End Sub

	'Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

	'	If (e.Error IsNot Nothing) Then
	'		MessageBox.Show(e.Error.Message)
	'	Else
	'		If e.Cancelled = True Then
	'			Me.btnSearch.Enabled = True
	'			Me.btnPrint.Enabled = True
	'			Me.btnExport.Enabled = True
	'			MessageBox.Show(TranslateMyText("Vorgang wurde abgebrochen!"))

	'		Else
	'			BackgroundWorker1.CancelAsync()
	'			'        MessageBox.Show(e.Result.ToString())

	'			If Not FormIsLoaded("frmYAHVListSearch_LV", True) Then
	'				frmMyLV = New frmYAHVListSearch_LV(ClsDataDetail.GetSQLQuery(), Me.Location.X, Me.Location.Y, Me.Height)

	'				frmMyLV.Show()
	'				Me.Select()
	'				' "Datenauflistung für {0} Einträge: in {1} ms"
	'				Me.LblTimeValue.Text = String.Format(TranslateMyText("Datenauflistung für {0} Einträge: in {1} ms"), _
	'																						 frmMyLV.LvFoundedrecs.Items.Count.ToString, _
	'																						 Stopwatch.ElapsedMilliseconds().ToString)

	'				' "{0} Datensätze wurden aufgelistet..."
	'				Me.LblState_1.Text = String.Format(TranslateMyText("{0} Datensätze wurden aufgelistet..."), _
	'																					 frmMyLV.LvFoundedrecs.Items.Count.ToString)
	'				frmMyLV.LblState_1.Text = String.Format(TranslateMyText("{0} Datensätze wurden aufgelistet..."), _
	'																								frmMyLV.LvFoundedrecs.Items.Count.ToString)

	'			End If
	'			Me.btnSearch.Enabled = True
	'			Me.btnPrint.Enabled = True
	'			Me.btnExport.Enabled = True
	'			Me.txt_SQL_2.Text = ClsDataDetail.GetSQLQuery()
	'		End If
	'	End If

	'End Sub

#End Region


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"AHV-Lohnbescheinigung drucken#PrintList", "AHV-freie Personen/Lohnsummen drucken#PrintListfree"}
		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))

				If myValue(1).ToString.ToLower = "PrintDesign".ToLower Then bshowMnu = IsUserActionAllowed(0, 560) Else bshowMnu = myValue(0).ToString <> String.Empty
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(1).ToString.ToLower = "PrintDesign".ToLower Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Art As Integer = 0

		Me.SQL4Print = Me.txt_SQL_2.Text
		Try
			Select Case e.Item.Name.ToUpper
				Case "PrintList".ToUpper
					Me.bPrintAsDesign = False
					Art = 0

				Case "PrintListfree".ToUpper
					Me.bPrintAsDesign = False
					Art = 1

				Case Else
					Exit Sub

			End Select
			Me.PrintJobNr = GetJobID(Art)
			StartPrinting()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiExport.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Alle Daten in CSV- / TXT exportieren...#CSV"}
		Try
			bbiExport.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiExport.ActAsDropDown = False
			Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiExport.DropDownEnabled = True
			Me.bbiExport.DropDownControl = popupMenu
			Me.bbiExport.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = myValue(1).ToString

					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("CSV")
				StartExportModul()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																			 .SQL2Open = Me.txt_SQL_2.Text,
																																			 .ModulName = "AHVLohnbescheinigungListToCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromJAHVListing(Me.txt_SQL_2.Text)

	End Sub

	Sub StartPrinting()
		bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)

		Dim strFilter As String = String.Empty

		strFilter &= String.Format("Mandant: {0}{1}", m_SearchCriteria.mandantenname, vbNewLine)
		strFilter &= If(m_SearchCriteria.FirstYear > 0, String.Format("Jahr: {0}{1}", m_SearchCriteria.FirstYear, vbNewLine), String.Empty)
		strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.filiale), String.Format("Filiale: {0}{1}", m_SearchCriteria.filiale, vbNewLine), String.Empty)
		strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.kanton), String.Format("Kanton: {0}{1}", m_SearchCriteria.kanton, vbNewLine), String.Empty)
		strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.nationality), String.Format("Nationalität: {0}{1}", m_SearchCriteria.nationality, vbNewLine), String.Empty)

		Dim _Setting As New ClsLLJAHVSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																													.SelectedMDNr = m_InitializationData.MDData.MDNr,
																													.SelectedMDYear = m_SearchCriteria.FirstYear,
																													.LogedUSNr = m_InitializationData.UserData.UserNr,
																													 .SQL2Open = Me.SQL4Print,
																													 .JobNr2Print = Me.PrintJobNr,
																													 .ListBez2Print = m_SearchCriteria.listname,
																													 .frmhwnd = GetHwnd,
																													 .ShowAsDesign = bPrintAsDesign,
		.ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)})
																													}
		Dim obj As New JAHVSearchListing.ClsPrintJAHVSearchList(_Setting)
		obj.PrintJAHVSearchList()

	End Sub


	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is ComboBoxEdit Then
				Dim cbo As ComboBoxEdit = CType(sender, ComboBoxEdit)
				cbo.EditValue = Nothing
			End If
		End If

	End Sub

End Class

