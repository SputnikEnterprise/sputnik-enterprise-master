
Option Strict Off

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports DevExpress.XtraEditors
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports System.Threading
Imports DevExpress.XtraEditors.Controls

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports DevExpress.LookAndFeel
Imports SPProgUtility.ProgPath




Public Class frmMFakListSearch
	Inherits XtraForm

	Protected Shared m_Logger As ILogger = New Logger()

#Region "Private fields"

	'Private _ClsFunc As New ClsDivFunc

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	Public Shared frmMyLV As frmListeSearch_LV
	Public Const frmMyLVName As String = "frmListeSearch_LV"

	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private m_mandant As Mandant
	Protected m_Utility As SPProgUtility.MainUtilities.Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Private m_SearchCriteria As New SearchCriteria

#End Region


#Region "Constructor"


	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_mandant = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		ResetMandantenDropDown()
		LoadMandantenDropDown()

		PrintJobNr = GetJobNr

	End Sub


#End Region

	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property

	Private ReadOnly Property GetJobNr() As String
		Get
			Return "11.6"

		End Get
	End Property


#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
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



	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmLohnkontiSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

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

	Private Sub OnForm_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		Try
			If FormIsLoaded(frmMyLVName, False) Then
				frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	''' <summary>
	'''  trannslate controls
	''' </summary>
	''' <remarks></remarks>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQLAbfrage.Text)
		Me.lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblSQLAbfrage.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

	End Sub






#Region "Dropdown Funktionen 1. Seite..."

	Private Sub Cbo_Month_1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Month_1.QueryPopUp
		ListLOMonth(Me.Cbo_Month_1)
	End Sub

	Private Sub Cbo_Month_2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Month_2.QueryPopUp
		ListLOMonth(Me.Cbo_Month_2)
	End Sub

	Private Sub Cbo_Year_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Year.QueryPopUp
		ListLOYear(Cbo_Year)
	End Sub

	Private Sub Cbo_Kanton_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Kanton.QueryPopUp
		ListMAKanton(Me.Cbo_Kanton, Me.Cbo_Year.Text)
	End Sub

	Private Sub Cbo_Filiale_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		Dim year As Integer? = Cbo_Year.EditValue
		If Not year.HasValue Then year = m_InitializationData.MDData.MDYear
		ListMAFiliale(Me.Cbo_Filiale, year)
	End Sub

	Private Sub Cbo_Nationality_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Nationality.QueryPopUp
		ListMANationality(Cbo_Nationality, Me.Cbo_Year.Text)
	End Sub

#End Region





	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmZGSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		SetInitialFields()
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

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

			If m_InitializationData.UserData.UserNr <> 1 Then Me.XtraTabControl1.TabPages.Remove(Me.xtabSQLAbfrage)

			bbiClear.Enabled = True
			bbiPrint.Enabled = False
			bbiExport.Enabled = False

			FillDefaultValues()

			Dim strSort As String = My.Settings.DefaultSortValue

			If strSort = String.Empty Then strSort = "2 - Kandidatenname"

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

			ListLOYear(Me.Cbo_Year)

			' Bis am 10. des Monats wird der Vormonat selektiert. Ab 11. den aktuellen Monat
			Dim datum As Date = Date.Now
			If datum.Day < 11 Then
				datum = datum.AddMonths(-1)
			End If
			Me.Cbo_Year.EditValue = datum.Year

			ListLOMonth(Me.Cbo_Month_1)
			ListLOMonth(Me.Cbo_Month_2)

			Me.Cbo_Month_1.EditValue = datum.Month
			'Me.Cbo_Month_2.EditValue = datum.Month

			GetInfoString4LA()

		Catch ex As Exception

		End Try

	End Sub

	Function GetInfoString4LA() As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strValue As String = String.Empty
		Dim strSqlQuery As String = "Select LA.LANr, LA.LALOText From LA Where MDNr = @MDNr And LANr In ("
		strSqlQuery += "3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3900.1, 3901, 3901.1) And LA.LAJahr = @Jahr "
		strSqlQuery += "Order By LA.LANr"

		'strSqlQuery = String.Format(strSqlQuery, Math.Max(CInt(Me.Cbo_Year.Text), Now.Year))
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", Cbo_Year.EditValue)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			ClsDataDetail.GetFilterBez2 = String.Empty
			While reader.Read
				ClsDataDetail.GetFilterBez2 &= If(ClsDataDetail.GetFilterBez2 = String.Empty, "", ", ") & String.Format("{0}", Format(reader("LANr"), "f"))
				strValue &= String.Format("<b>{0}:</b> {1}</br>", Format(reader("LANr"), "f"), reader("LALOText"))

			End While
			Me.lbLstlInfo.Text = String.Format("<size=11><b>" & m_Translate.GetSafeTranslationValue("Info") & "</b><size=8.2></br>" &
																				 m_Translate.GetSafeTranslationValue("Nach folgenden Lohnarten wird gesucht:</br>{0}"),
																				 strValue)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return strValue

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strValue
	End Function

#Region "zum Leeren..."

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClearFields_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

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


	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick

		Try
			m_SearchCriteria = GetSearchKrieteria()
			If Not (Kontrolle()) Then Exit Sub

			Me.bbiSearch.Enabled = False
			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

			Me.txt_SQLQuery.Text = String.Empty
			ClsDataDetail.GetSQLQuery() = String.Empty
			ClsDataDetail.GetSQLSortString() = String.Empty

			FormIsLoaded(frmMyLVName, True)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			GetMyQueryString()

		Catch ex As Exception
			MessageBox.Show(ex.Message, "bbiSearch_Click")
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		Finally

		End Try

	End Sub

	Function Kontrolle() As Boolean
		Try
			Dim msg As String = ""

			Dim jahrVon As Integer = m_SearchCriteria.firstYear
			Dim monthVon As Integer = m_SearchCriteria.firstMonth

			If lueMandant.EditValue Is Nothing Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben keinen Mandanten ausgewählt.{0}"), vbNewLine)
			End If

			If Not jahrVon > 0 Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben kein Jahr ausgewählt.{0}"), vbNewLine)
			End If

			If Not monthVon > 0 Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben keinen Monat ausgewählt.{0}"), vbNewLine)
			End If

			If Not String.IsNullOrWhiteSpace(msg) Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien unvollständig:{0}{1}"), vbNewLine, msg)
				m_UtilityUi.ShowErrorDialog(msg)

				Return False
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

		Return True
	End Function

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria


		result.listname = m_Translate.GetSafeTranslationValue("Kinder- und Ausbildungszulagen")
		result.mandantenname = lueMandant.Text

		Dim lastyear As Integer? = Me.Cbo_Year.EditValue
		If Not lastyear.HasValue Then lastyear = Cbo_Year.EditValue

		Dim lastmonth As Integer? = Me.Cbo_Month_2.EditValue
		If Not lastmonth.HasValue Then lastmonth = CType(Cbo_Month_1.EditValue, Integer)

		result.firstYear = Cbo_Year.EditValue
		result.lastYear = lastyear.Value

		result.firstMonth = Cbo_Month_1.EditValue
		result.lastMonth = lastmonth.Value

		result.s_kanton = Cbo_Kanton.EditValue
		result.filiale = Cbo_Filiale.EditValue
		result.nationality = Cbo_Nationality.EditValue

		m_SearchCriteria.sqlsearchstring = String.Empty

		Return result

	End Function

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim strArtQuery As String = String.Empty

		BackgroundWorker1.WorkerSupportsCancellation = True
		BackgroundWorker1.RunWorkerAsync()

		'End If

		Return True
	End Function



#Region "Key Ereignisse..."

	Private Sub InitClickHandler(ByVal ParamArray Ctrls() As Control)

		For Each Ctrl As Control In Ctrls
			AddHandler Ctrl.KeyPress, AddressOf KeyPressEvent
			'      AddHandler Ctrl.Click, AddressOf ClickEvents
		Next

	End Sub

	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs)	' System.EventArgs)
		'   ToDo  Auswertung und Klick-Aktion ausführen
		'If sender Is TextBox1 Then

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

		'End If
	End Sub

#End Region


	Private Sub frmMFakListSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded(frmMyLVName, False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

#Region "Multitreading..."

	Private Sub OnBgWorkerDoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
		'Dim _ClsDb As New ClsDbFunc(m_SearchCriteria)
		Dim sSql1Query As String = String.Empty
		Dim _ClsDb As New ClsDbFunc(m_SearchCriteria)
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		CheckForIllegalCrossThreadCalls = False
		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)
		Dim i As Integer = 0
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

		Me.bbiSearch.Enabled = False

		Try
			sSql1Query = _ClsDb.GetStartSQLString()		 ' 1. String
			sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query)		' Where Klausel

			If Trim(sSql2Query) <> String.Empty Then sSql1Query += " Where "

			strSort = _ClsDb.GetSortString()			' Sort Klausel
			ClsDataDetail.GetSQLSortString = strSort
			ClsDataDetail.GetSQLQuery = sSql1Query & " " & sSql2Query & " " & strSort

			Me.txt_SQLQuery.Text = ClsDataDetail.GetSQLQuery

			Dim sSqlQuerySelect As String = Me.txt_SQLQuery.Text
			m_SearchCriteria.sqlsearchstring = Me.txt_SQLQuery.Text

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		Finally
			e.Result = True
			If bw.CancellationPending Then e.Cancel = True

		End Try

	End Sub

	Private Sub OnBgWorkerProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub OnBgWorkerRunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(e.Error.Message)
		Else
			If e.Cancelled = True Then
				Me.bbiSearch.Enabled = True
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True
				MessageBox.Show(m_Translate.GetSafeTranslationValue("Vorgang wurde abgebrochen!"))

			Else
				BackgroundWorker1.CancelAsync()
				Dim reccount As Integer = 0

				' Daten auflisten...
				If Not String.IsNullOrWhiteSpace(m_SearchCriteria.sqlsearchstring) Then
					If Not FormIsLoaded(frmMyLVName, True) Then
						frmMyLV = New frmListeSearch_LV(Me.txt_SQLQuery.Text, String.Empty)
						frmMyLV.Show()
						frmMyLV.BringToFront()
					End If
					reccount = frmMyLV.RecCount
					frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...", _
																					reccount)
				End If

				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...", _
																					 reccount)

				' Die Buttons Drucken und Export aktivieren
				Me.bbiPrint.Enabled = reccount > 0
				Me.bbiExport.Enabled = reccount > 0
				If reccount > 0 Then
					'CreatePrintPopupMenu()
					CreateExportPopupMenu()
				End If

			End If
		End If
		bbiClear.Enabled = True
		Me.bbiSearch.Enabled = True
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

	End Sub


#End Region



	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		StartPrinting()
		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)
		'If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

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
																																			 .SQL2Open = Me.txt_SQLQuery.Text,
																																			 .ModulName = "MFakListToCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromMFakListing(Me.txt_SQLQuery.Text)

	End Sub

	Sub StartPrinting()
		Dim strFilter As String = String.Empty

		Try

			bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)
			PrintJobNr = GetJobNr
			SQL4Print = Me.txt_SQLQuery.Text

			strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname)

			strFilter &= If(m_SearchCriteria.firstMonth > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.firstMonth, vbNewLine), String.Empty)
			strFilter &= If(m_SearchCriteria.firstYear > 0, String.Format(" / {0}", m_SearchCriteria.firstYear, vbNewLine), String.Empty)

			strFilter &= If(m_SearchCriteria.lastMonth > 0, String.Format(" - {0}", m_SearchCriteria.lastMonth), String.Empty)
			strFilter &= If(m_SearchCriteria.lastYear > 0, String.Format(" / {0}", m_SearchCriteria.lastYear), String.Empty)

			strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.filiale), String.Format("{1}" & m_Translate.GetSafeTranslationValue("Filiale: {0}"), m_SearchCriteria.filiale, vbNewLine), String.Empty)
			strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.s_kanton), String.Format("{1}" & m_Translate.GetSafeTranslationValue("Kanton: {0}"), m_SearchCriteria.s_kanton, vbNewLine), String.Empty)
			strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.nationality), String.Format("{1}" & m_Translate.GetSafeTranslationValue("Nationalität: {0}"), m_SearchCriteria.nationality, vbNewLine), String.Empty)

			Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMFakSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																									 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																																									 .SelectedMDYear = m_SearchCriteria.firstYear,
																																									 .LogedUSNr = m_InitializationData.UserData.UserNr,
																																									 .SQL2Open = SQL4Print,
																																									 .frmhwnd = GetHwnd,
																																									 .ShowAsDesign = bPrintAsDesign,
																																									 .JobNr2Print = PrintJobNr,
																																									 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)})}

			ClsDataDetail.GetFilterBez = m_SearchCriteria.listname
			ClsDataDetail.GetFilterBez2 = strFilter
			Dim obj As New SPS.Listing.Print.Utility.MFakSearchListing.ClsPrintMFakSearchList(m_InitializationData)

			obj.PrintMFakListing(_Setting)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub



	'Private Sub Cbo_Year_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

	'	Me.Cbo_Kanton.Text = String.Empty
	'	Me.Cbo_Nationality.Text = String.Empty

	'	Me.Cbo_Kanton.Properties.Items.Clear()
	'	Me.Cbo_Nationality.Properties.Items.Clear()

	'End Sub



	'Private Sub bbiPrint_DropDownOpened(sender As Object, e As System.EventArgs)
	'	Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
	'	For Each itm As ToolStripItem In ts.DropDownItems
	'		itm.Text = m_Translate.GetSafeTranslationValue(itm.Text)
	'		Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
	'	Next
	'End Sub

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.Cbo_Month_2.Visible = Me.SwitchButton1.Value
		Me.Cbo_Month_2.EditValue = Nothing
	End Sub


End Class

