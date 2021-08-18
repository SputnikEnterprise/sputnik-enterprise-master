
Imports System.Reflection.Assembly

Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports System.Data.SqlClient
Imports System.Threading
Imports SPS.Listing.Print.Utility
Imports System.Reflection
Imports SPProgUtility.ProgPath

Public Class frmGAVInkassoSearch
	Inherits XtraForm


#Region "private Consts"

	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private Shared m_Logger As ILogger = New Logger()
	Private m_connectionString As String

	Private m_mandant As Mandant
	Private m_path As ClsProgPath

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility
	Private m_utilitySP As SPProgUtility.MainUtilities.Utilities

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

		m_InitializationData = _setting
		m_mandant = New Mandant
		m_path = New ClsProgPath
		m_Utility = New Utility
		m_utilitySP = New SPProgUtility.MainUtilities.Utilities
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		ClsDataDetail.m_InitialData = m_InitializationData
		ClsDataDetail.m_Translate = m_Translate

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		TranslateControls()

		Reset()
		LoadMandantenDropDown()

		lblVor2012.Visible = Not IsNoPVL

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
			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

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

	Private ReadOnly Property GetJobID() As String
		Get
			Return "11.0.1"
		End Get
	End Property

	Private ReadOnly Property IsNoPVL() As Boolean
		Get

			Dim mandantNumber As Integer = m_InitializationData.MDData.MDNr
			Dim companyallowednopvl As Boolean? = m_path.ParseToBoolean(m_path.GetXMLNodeValue(m_mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																																												 String.Format("{0}/companyallowednopvl", FORM_XML_MAIN_KEY)), False)

			Return If(companyallowednopvl Is Nothing, False, companyallowednopvl)

		End Get
	End Property


#Region "Private Methods"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)
		CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)

		lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)
		lblMANr.Text = m_Translate.GetSafeTranslationValue(lblMANr.Text)

		lblPerdiode.Text = m_Translate.GetSafeTranslationValue(lblPerdiode.Text)
		lblJahr.Text = m_Translate.GetSafeTranslationValue(lblJahr.Text)
		lblVor2012.Text = m_Translate.GetSafeTranslationValue(lblVor2012.Text)
		lblmonat.Text = m_Translate.GetSafeTranslationValue(lblmonat.Text)

		lblkanton.Text = m_Translate.GetSafeTranslationValue(lblkanton.Text)
		lblBeruf.Text = m_Translate.GetSafeTranslationValue(lblBeruf.Text)

		xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(xtabAllgemein.Text)
		xtabSQLQuery.Text = m_Translate.GetSafeTranslationValue(xtabSQLQuery.Text)
		lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(lblSQLAbfrage.Text)

		beiWorking.Caption = m_Translate.GetSafeTranslationValue(beiWorking.Caption)
		bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)
		bbiSearch.Caption = m_Translate.GetSafeTranslationValue(bbiSearch.Caption)
		bbiClearFields.Caption = m_Translate.GetSafeTranslationValue(bbiClearFields.Caption)
		bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)
		bbiExport.Caption = m_Translate.GetSafeTranslationValue(bbiExport.Caption)

	End Sub


#End Region


#Region "Dropdown Funktionen Allgemein"

	''' <summary>
	''' Selektionsfenster für die erste Mitarbeiter-Nummer öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub txt_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANr.ButtonClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim frmTest As New frmSearchRec("MANr")

		Dim m As String

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iListeValue()
		Me.txt_MANr.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub OnYear_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_JahrVon.QueryPopUp
		Try
			If DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit).Properties.Items.Count = 0 Then ListLOJahr(sender)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub OnMonth_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VMonth_1.QueryPopUp, Cbo_BMonth_1.QueryPopUp
		Try
			ListMonth(sender, Cbo_JahrVon.EditValue)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub OnGAVKanton_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Kanton.QueryPopUp
		LoadGAVCanton(Val(Cbo_JahrVon.EditValue), Val(Cbo_VMonth_1.EditValue), Val(Cbo_BMonth_1.EditValue), Cbo_FARListeBeruf.EditValue)
	End Sub

	Private Sub OnGAVBeruf_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_FARListeBeruf.QueryPopUp

		LoadGAVBerufe(Val(Cbo_JahrVon.EditValue), Val(Cbo_VMonth_1.EditValue), Val(Cbo_BMonth_1.EditValue), Cbo_Kanton.EditValue)

	End Sub

	Private Sub OnCbo_Periode_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Periode.QueryPopUp
		Dim datum As Date = Date.Now
		Me.Cbo_Periode.Properties.Items.Clear()

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Diesen Monat ({0:00}/{1})"), datum.Month, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0:00}/{1})"),
																												datum.AddMonths(-1).Month, datum.AddMonths(-1).Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 1, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 2, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 3, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 4, datum.Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letztes Jahr ({0})"), datum.AddYears(-1).Year))
	End Sub

	Private Sub Cbo_Periode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_Periode.SelectedIndexChanged
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim dauer As Integer = 0
			Dim datum As Date = Date.Now
			Dim index1 As Integer = datum.Month - 1
			Dim bSetSelectedDate As Boolean = True
			Me.Cbo_BMonth_1.Visible = False
			Me.SwitchButton1.Value = False

			Select Case Me.Cbo_Periode.SelectedIndex
				'Case 0
				'  bSetSelectedDate = False

				Case 2 '"1Q"
					index1 = 0
					dauer = 2
				Case 3 ' "2Q"
					index1 = 3
					dauer = 2
				Case 4 ' "3Q"
					index1 = 6
					dauer = 2
				Case 5 ' "4Q"
					index1 = 9
					dauer = 2
					'Case "1H"
					'  dauer = 5
					'Case "2H"
					'  index1 = 6
					'  dauer = 5
				Case 0 ' "DM"
					index1 = datum.Month - 1
					dauer = 0
				Case 1 ' "LM"
					datum = datum.AddMonths(-1)
					index1 = datum.Month - 1
					dauer = 0
				Case 6 ' "LJ"
					index1 = 0
					dauer = 11
					datum = datum.AddYears(-1)
			End Select

			If bSetSelectedDate Then
				Me.SwitchButton1.Value = True
				Me.Cbo_JahrVon.EditValue = datum.Year

				Me.Cbo_VMonth_1.EditValue = index1 + 1
				Me.Cbo_BMonth_1.EditValue = index1 + 1 + dauer
				Me.Cbo_BMonth_1.Visible = True

			Else
				FillDefaultDates()

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))
		End Try

	End Sub

	Private Sub OnMonthSwitchButton(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.Cbo_BMonth_1.Visible = Me.SwitchButton1.Value
		Me.Cbo_BMonth_1.EditValue = Nothing
	End Sub

	Private Sub Cbo_FARListeBeruf_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_FARListeBeruf.SelectedIndexChanged
		Chk_GAVDivBerufPauschale.EditValue = False
		Chk_GAVDivBerufPauschale.Visible = ShowPauschale(Cbo_FARListeBeruf.EditValue, Val(Cbo_VMonth_1.EditValue), Val(Cbo_BMonth_1.EditValue), Val(Cbo_JahrVon.EditValue), Cbo_Kanton.EditValue, txt_MANr.EditValue)
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
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
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

			ListLOJahr(Me.Cbo_JahrVon)

			' Bis am 10. des Monats wird der Vormonat selektiert. Ab 11. den aktuellen Monat
			Dim datum As Date = Date.Now
			If datum.Day < 11 Then
				datum = datum.AddMonths(-1)
			End If
			Me.Cbo_JahrVon.EditValue = datum.Year

			ListMonth(Me.Cbo_VMonth_1, Cbo_JahrVon.EditValue)
			ListMonth(Me.Cbo_BMonth_1, Cbo_JahrVon.EditValue)

			Me.Cbo_VMonth_1.EditValue = datum.Month
			Me.Cbo_BMonth_1.EditValue = Nothing


		Catch ex As Exception

		End Try

	End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick

		Try

			FormIsLoaded(frmMyLVName, True)

			m_SearchCriteria = GetSearchKrieteria()
			If Not (Kontrolle()) Then Exit Sub
			Me.txt_SQLQuery.Text = String.Empty


			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."


			' Die Query-String aufbauen...
			GetMyQueryString()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally

		End Try

	End Sub

	Function Kontrolle() As Boolean
		Dim msg As String = ""

		Dim jahrVon As Integer? = m_SearchCriteria.FirstYear
		Dim monatVon As Integer? = m_SearchCriteria.FirstMonth

		Try

			If lueMandant.EditValue Is Nothing Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben keinen Mandanten ausgewählt.{0}"), vbNewLine)
			End If

			If Not jahrVon.HasValue Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben kein Jahr ausgewählt.{0}"), vbNewLine)
			End If

			If Not monatVon.HasValue Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben keinen Monat ausgewählt.{0}"), vbNewLine)
			End If

			If Me.Cbo_FARListeBeruf.EditValue Is Nothing Then
				'msg += String.Format(m_Translate.GetSafeTranslationValue("Sie haben keinen GAV-Beruf ausgewählt."), vbNewLine, msg)
			End If


			If Not String.IsNullOrWhiteSpace(msg) Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien unvollständig:{0}{1}"), vbNewLine, msg)
				m_UtilityUI.ShowErrorDialog(msg)

				Return False

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return False
		End Try

		Return True
	End Function

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		result.listname = m_Translate.GetSafeTranslationValue("PVL-Liste")
		result.mandantenname = lueMandant.Text

		result.MANrList = txt_MANr.EditValue

		If Me.Cbo_JahrVon.EditValue Is Nothing OrElse Val(Me.Cbo_JahrVon.EditValue) = 0 Then Me.Cbo_JahrVon.EditValue = Now.Year
		If Me.Cbo_VMonth_1.EditValue Is Nothing OrElse Val(Me.Cbo_VMonth_1.EditValue) = 0 Then Me.Cbo_VMonth_1.EditValue = Now.Month
		Dim lastmonth As Integer = If(Cbo_BMonth_1.EditValue Is Nothing OrElse Val(Cbo_BMonth_1.EditValue) = 0, Cbo_VMonth_1.EditValue, Cbo_BMonth_1.EditValue)

		result.FirstYear = Cbo_JahrVon.EditValue
		result.FirstMonth = Cbo_VMonth_1.EditValue
		result.LastMonth = lastmonth

		result.kanton = Cbo_Kanton.EditValue
		result.beruf = Cbo_FARListeBeruf.EditValue

		m_SearchCriteria.sqlsearchstring = String.Empty


		Return result

	End Function

	Function GetMyQueryString() As Boolean

		Try
			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.WorkerReportsProgress = True
			BackgroundWorker1.RunWorkerAsync()

		Catch ex As Exception
			Return False

		End Try

		Return True
	End Function



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

		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

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



#Region "Multitreading..."

	Private Sub OnBgWorkerDoWork(ByVal sender As System.Object,
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
			Me.txt_SQLQuery.Text = _ClsDb.GetStartSQLString()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		Finally
			e.Result = True
			If bw.CancellationPending Then e.Cancel = True
			Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		End Try

	End Sub

	Private Sub OnBgWorkerProgressChanged(ByVal sender As Object,
																								ByVal e As System.ComponentModel.ProgressChangedEventArgs) _
																								Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub OnBgWorkerRunWorkerCompleted(ByVal sender As Object,
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
					frmMyLV = New frmListeSearch_LV(Me.txt_SQLQuery.Text, String.Empty)
					frmMyLV.Show()
					frmMyLV.BringToFront()
				End If

				Dim reccount As Integer = frmMyLV.RecCount
				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																					 reccount)
				frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																								reccount)

				' Die Buttons Drucken und Export aktivieren
				Me.bbiPrint.Enabled = reccount > 0
				Me.bbiExport.Enabled = reccount > 0
				If reccount > 0 Then
					CreateExportPopupMenu()
				End If

			End If
		End If
		Me.bbiSearch.Enabled = True
		Me.bbiClearFields.Enabled = True
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

	End Sub

#End Region


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		StartPrinting()
	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiExport.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Daten in CSV- / TXT exportieren...#CSV"}
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
		Dim strSQL As String = Me.txt_SQLQuery.Text

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("TXT"), UCase("CSV")
				StartExportModul()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																			 .SQL2Open = Me.txt_SQLQuery.Text,
																																			 .ModulName = "InkassopoolListToCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromInkassopoolListing(Me.txt_SQLQuery.Text)

	End Sub

	Sub StartPrinting()
		bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)

		Dim strFilter As String = String.Empty
		Dim Jahreslohnsumme As Decimal = GetJahreslohnsumme(m_SearchCriteria.FirstYear, m_SearchCriteria.beruf)

		Me.PrintJobNr = GetJobID()
		Me.SQL4Print = Me.txt_SQLQuery.Text

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname) & vbNewLine

		strFilter &= If(Not m_SearchCriteria.MANrList Is Nothing, String.Format("{1}Kandidaten: {0}", m_SearchCriteria.MANrList, vbNewLine), String.Empty)

		strFilter &= If(Not m_SearchCriteria.FirstMonth > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.FirstMonth, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.LastMonth > 0, String.Format(" - {0}", m_SearchCriteria.LastMonth), String.Empty)

		strFilter &= If(m_SearchCriteria.FirstYear > 0, String.Format(" / {0}", m_SearchCriteria.FirstYear, vbNewLine), String.Empty)

		strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.beruf), String.Format("{1}GAV-Beruf: {0}", m_SearchCriteria.beruf, vbNewLine), String.Empty)

		Dim _Setting As New ClsLLPVLSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																												 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																												 .LogedUSNr = m_InitializationData.UserData.UserNr,
																													.SelectedMDYear = m_SearchCriteria.FirstYear,
																												 .Jahreslohnsumme = Jahreslohnsumme,
																												 .PauschaleChecked = Me.Chk_GAVDivBerufPauschale.Checked,
																												 .SQL2Open = Me.SQL4Print,
																												 .JobNr2Print = Me.PrintJobNr,
																												 .ListBez2Print = m_SearchCriteria.listname,
																												 .frmhwnd = GetHwnd,
																												 .ShowAsDesign = Me.bPrintAsDesign,
																												 .GAVBezeichnung = Cbo_FARListeBeruf.EditValue,
																												 .SearchAddressDataFromTableseting = False,
																												 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)})
		}

		Dim obj As New PVLSearchListing.ClsPrintPVLSearchList(_Setting)
		obj.PrintPVLSearchList()

	End Sub

	Private Sub LoadGAVBerufe(ByVal year As Integer, ByVal MonatVon As Integer, ByVal MonatBis As Integer, ByVal kanton As String)
		Dim strEntry As String
		Dim strSqlQuery As String
		If year <= 2012 Then strSqlQuery = "[List InkassoPool GAVBeruf With Mandant]" Else strSqlQuery = "[List InkassoPool GAVBeruf With Mandant_2012]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", year)
			param = cmd.Parameters.AddWithValue("@LPVon", If(MonatVon = 0, Now.Month, MonatVon))
			param = cmd.Parameters.AddWithValue("@LPBis", If(MonatBis = 0, If(MonatVon = 0, Now.Month, MonatVon), MonatBis))
			param = cmd.Parameters.AddWithValue("GavKanton", m_utilitySP.ReplaceMissing(kanton, String.Empty))

			Dim reader As SqlDataReader = cmd.ExecuteReader

			Cbo_FARListeBeruf.Properties.Items.Clear()
			While reader.Read
				strEntry = m_utilitySP.SafeGetString(reader, "GAVBeruf")

				Cbo_FARListeBeruf.Properties.Items.Add(strEntry)

			End While
			Cbo_FARListeBeruf.Properties.DropDownRows = 13


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Private Sub LoadGAVCanton(ByVal year As Integer, ByVal MonatVon As Integer, ByVal MonatBis As Integer, ByVal gavberuf As String)
		Dim strEntry As String
		Dim strSqlQuery As String = "[List InkassoPool GAVKanton With Mandant]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", year)
			param = cmd.Parameters.AddWithValue("@LPVon", If(MonatVon = 0, Now.Month, MonatVon))
			param = cmd.Parameters.AddWithValue("@LPBis", If(MonatBis = 0, If(MonatVon = 0, Now.Month, MonatVon), MonatBis))
			param = cmd.Parameters.AddWithValue("Gavberuf", m_utilitySP.ReplaceMissing(gavberuf, String.Empty))

			Dim reader As SqlDataReader = cmd.ExecuteReader

			Cbo_Kanton.Properties.Items.Clear()
			While reader.Read
				strEntry = m_utilitySP.SafeGetString(reader, "GAVKanton")

				Cbo_Kanton.Properties.Items.Add(strEntry)

			End While
			Cbo_Kanton.Properties.DropDownRows = 13


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Private Sub ListLOJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As Integer?
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities
		Dim strSqlQuery As String = "Select LO.Jahr From LO Where LO.Jahr > 2006 "
		strSqlQuery += "And MDNr = @MDNr And Jahr Is Not Null Group By LO.Jahr Order By LO.Jahr DESC"

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				strEntry = m_utility.SafeGetInteger(reader, "Jahr", 0)

				cbo.Properties.Items.Add(strEntry)

			End While

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Private Sub ListMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal year As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strFieldName As String = "LP"
		Dim strSqlQuery As String = String.Empty
		Dim strEntry As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "Select LP From LO Where MDNr = @MDNr And Jahr = @Jahr Group By LP Order By LP"
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", year)
			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				strEntry = CInt(m_utilitySP.SafeGetInteger(reader, strFieldName, 0))

				cbo.Properties.Items.Add(strEntry)

			End While
			cbo.Properties.DropDownRows = 13

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function GetJahreslohnsumme(ByVal jahr As Integer, ByVal beruf As String) As Decimal
		Dim vorjaherslohnsumme As Decimal = 0

		If beruf Is Nothing OrElse String.IsNullOrWhiteSpace(beruf) Then Return vorjaherslohnsumme

		Dim Sql As String = "[Get Jahreslohnsumme InkassoPool With Mandant]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitializationData.MDData.MDNr))
		listOfParams.Add(New SqlClient.SqlParameter("@jahr", m_utilitySP.ReplaceMissing(jahr, Now.Year)))
		listOfParams.Add(New SqlClient.SqlParameter("@gavBeruf", m_utilitySP.ReplaceMissing(beruf, String.Empty)))

		Try
			Dim reader As SqlDataReader = m_utilitySP.OpenReader(m_InitializationData.MDData.MDDbConn, Sql, listOfParams, Data.CommandType.StoredProcedure)
			If (Not reader Is Nothing AndAlso reader.Read()) Then
				vorjaherslohnsumme = m_utilitySP.SafeGetDecimal(reader, "Lohnsumme", 0)
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return vorjaherslohnsumme

		Finally

		End Try

		Return vorjaherslohnsumme

	End Function

	''' <summary>
	''' Gibt True zurück, wenn für die angegebene Zeitperiode, GAV-Beruf, -Kanton und Kandidatennummer
	''' eine Pauschale vorhanden ist.
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function ShowPauschale(ByVal gavBeruf As String, ByVal firstmonat As Integer, ByVal lastmonat As Integer, ByVal year As Integer, ByVal gavcanton As String, ByVal employeenumbers As String) As Boolean


		Dim pauschale = m_ListingDatabaseAccess.LoadInkassopoolPauschale(lueMandant.EditValue, gavBeruf, gavcanton, firstmonat, lastmonat, year, employeenumbers)


		Return Not pauschale Is Nothing AndAlso pauschale > 0

	End Function


#Region "Helpers"

	Private Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
		Try
			Dim bResult As Boolean = False

			' alle geöffneten Forms durchlaufen
			For Each oForm As Form In Application.OpenForms
				If oForm.Name.ToLower = sName.ToLower Then
					If bDisposeForm Then oForm.Dispose() : Exit For
					bResult = True : Exit For
				End If
			Next

			Return (bResult)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False

		End Try

	End Function

	Private Shared Function MyResolveEventHandler(sender As Object, args As ResolveEventArgs) As Assembly
		'This handler is called only when the common language runtime tries to bind to the assembly and fails.        
		m_Logger = New Logger

		'Retrieve the list of referenced assemblies in an array of AssemblyName.
		Dim objExecutingAssemblies As [Assembly]
		objExecutingAssemblies = [Assembly].GetExecutingAssembly()
		Dim arrReferencedAssmbNames() As AssemblyName
		arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies()

		'Loop through the array of referenced assembly names.
		Dim strAssmbName As AssemblyName
		For Each strAssmbName In arrReferencedAssmbNames


			'Look for the assembly names that have raised the "AssemblyResolve" event.
			If (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) = args.Name.Substring(0, args.Name.IndexOf(","))) Then
				Trace.WriteLine("1:" & args.Name.Substring(0, args.Name.IndexOf(",")) & ".dll")
				Trace.WriteLine("2:" & strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")))

				'Build the path of the assembly from where it has to be loaded.
				Dim strTempAssmbPath As String = String.Empty
				strTempAssmbPath = IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), args.Name.Substring(0, args.Name.IndexOf(",")) & ".dll")

				If IO.File.Exists(strTempAssmbPath) Then
					Dim msg = String.Format("loading Assembly: {0}", strTempAssmbPath)
					m_Logger.LogWarning(msg)
					Trace.WriteLine(String.Format("loading Assembly: ", strTempAssmbPath))
					Dim MyAssembly As [Assembly]

					'Load the assembly from the specified path. 
					MyAssembly = [Assembly].LoadFrom(strTempAssmbPath)

					'Return the loaded assembly.
					Return MyAssembly
				Else
					Dim msg = String.Format("Assembly could not be found: {0}", strTempAssmbPath)
					m_Logger.LogWarning(msg)
					Trace.WriteLine(msg)
				End If

			End If
		Next

		Return Nothing

	End Function

#End Region


End Class
