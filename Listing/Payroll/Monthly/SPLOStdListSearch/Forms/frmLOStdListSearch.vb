
Option Strict Off

Imports System.Reflection.Assembly
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports System.Data.SqlClient
Imports System.Threading
Imports SP.DatabaseAccess.Common
Imports SPS.Listing.Print.Utility

Public Class frmLOStdListSearch
	Inherits XtraForm

#Region "Private Fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_Connectionstring As String

	Private m_mandant As Mandant


	Public Shared frmMyLV As frmListeSearch_LV

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	'Private PrintListingThread As Thread
	Private m_SearchCriteria As New SearchCriteria


#Region "private properties"

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

#End Region


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

		m_Connectionstring = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_Connectionstring, m_InitializationData.UserData.UserLanguage)

		m_mandant = New Mandant
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		TranslateControls()

		Reset()
		LoadMandantenDropDown()

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

	Private ReadOnly Property GroupingList() As Boolean
		Get
			If (m_SearchCriteria.FirstMonth <> m_SearchCriteria.LastMonth) OrElse (m_SearchCriteria.FirstYear <> m_SearchCriteria.LastYear) Then
				Return True
			End If

			Return False
		End Get
	End Property


	Private ReadOnly Property GetJobID() As String
		Get
			Return String.Format("11.5{0}", If(GroupingList, ".G", ""))
		End Get
	End Property


#Region "Dropdown Funktionen Allgemein"

	' Jahr
	Private Sub ListLOJahr_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_JahrVon.QueryPopUp, Cbo_JahrBis.QueryPopUp
		Try
			If DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit).Properties.Items.Count = 0 Then ListLOJahr(sender)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	' Geschäftsstellen
	Private Sub Cbo_MAGeschaeftsstellen_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		If Me.Cbo_Filiale.Properties.Items.Count = 0 Then ListEmployeeFiliale(Cbo_Filiale)
	End Sub


#End Region


#Region "Allgemeine Funktionen"

	' Monate 1 bis 12
	Private Sub OnCbo_MonatVon_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MonatVon.QueryPopUp
		Try
			If Cbo_MonatVon.Properties.Items.Count = 0 Then ListCboMonate1Bis12(Cbo_MonatVon)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub OnCbo_MonatBis_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MonatBis.QueryPopUp
		Try
			If Cbo_MonatBis.Properties.Items.Count = 0 Then ListCboMonate1Bis12(Cbo_MonatBis)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub


#End Region


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
		Me.lblMANr.Text = m_Translate.GetSafeTranslationValue(Me.lblMANr.Text)

		Me.grpInfo.Text = m_Translate.GetSafeTranslationValue(Me.grpInfo.Text)
		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)

		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblSQLAbfrage.Text)

		Me.lblFiliale.Text = m_Translate.GetSafeTranslationValue(Me.lblFiliale.Text)
		Me.lblKanton.Text = m_Translate.GetSafeTranslationValue(Me.lblKanton.Text)
		Me.lblBeruf.Text = m_Translate.GetSafeTranslationValue(Me.lblBeruf.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClearFields.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClearFields.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

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

	Private Sub frmESSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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

			If Me.Cbo_MonatVon.Properties.Items.Count = 0 Then
				ListCboMonate1Bis12(Me.Cbo_MonatVon)
				ListCboMonate1Bis12(Me.Cbo_MonatBis)
				ListLOJahr(Me.Cbo_JahrVon)
				ListLOJahr(Me.Cbo_JahrBis)
			End If

			Dim datum As Date = Date.Now
			If datum.Day < 11 Then
				datum = datum.AddMonths(-1)
			End If

			Me.Cbo_MonatVon.Text = datum.Month.ToString
			Me.Cbo_MonatBis.EditValue = Nothing
			Me.Cbo_JahrBis.EditValue = Nothing
			Me.Cbo_JahrVon.EditValue = datum.Year
			Me.Cbo_MonatVon.EditValue = datum.Month

		Catch ex As Exception

		End Try

	End Sub

	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	''' <param name="bForExport">ob die Liste für Export ist.</param>
	''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	''' <remarks></remarks>
	Sub GetLOAN4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iESNr As Integer = 0
		Dim bResult As Boolean = True
		Dim storedProc As String = ""

		Dim sSql As String = Me.txt_SQLQuery.Text
		If String.IsNullOrWhiteSpace(sSql) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet."))
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim cmd As New SqlCommand(String.Format("SELECT * FROM {0}", ClsDataDetail.LLTablename), Conn)


		Try
			Conn.Open()

			If bForDesign Then
				sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 10 ", 1, 1)
			Else
				' Für die Fortschrittsanzeige im LL
				ClsDataDetail.AnzMax = 0
				Dim countReader As SqlDataReader = cmd.ExecuteReader
				While countReader.Read
					ClsDataDetail.AnzMax += 1
				End While
				countReader.Close()
			End If

			Dim reader As SqlDataReader = cmd.ExecuteReader									 ' 
			Try
				If Not reader.HasRows Then
					cmd.Dispose()
					reader.Close()

					m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))
				End If

			Catch ex As Exception


			End Try


			reader.Read()
			If reader.HasRows Then
				Me.SQL4Print = sSql
				Me.bPrintAsDesign = bForDesign
				Me.PrintJobNr = strJobInfo

				StartPrinting()

				'PrintListingThread = New Thread(AddressOf StartPrinting)
				'PrintListingThread.Name = "PrintingStdListing"
				'PrintListingThread.SetApartmentState(ApartmentState.STA)
				'PrintListingThread.Start()

			End If
			reader.Close()



			'While reader.Read

			'	'If LL.Core.Handle <> 0 Then LL.Core.LlJobClose()
			'	'LL.Core.LlJobOpen(2)

			'	If bForExport Then
			'		'            ExportLLDoc(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex),CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
			'		'          bResult = ExportLLDocWithStorage(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex), CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
			'	Else
			'		If bForDesign Then
			'			ShowInDesign(LL, strJobInfo, rQSTListerec)

			'		Else
			'			bResult = ShowInPrint(LL, strJobInfo, rQSTListerec)

			'		End If
			'	End If

			'	If Not bResult Or bForDesign Then Exit While
			'End While
			'reader.Close()

		Catch ex As Exception


		Finally
			cmd.Dispose()
			Conn.Close()

		End Try
	End Sub


#Region "Funktionen zur Menüaufbau..."

	'Private Sub mnuListeDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuListeDrucken.Click
	'	Try
	'		Dim jobnr As String = "11.5"

	'		' Wenn die Liste der Arbeitsstunden über meherere Monate, so die gruppierte Liste
	'		If ((Me.Cbo_JahrVon.Text <> Me.Cbo_JahrBis.Text And _
	'				 Me.Cbo_JahrBis.Text.Length > 0 And Me.Cbo_JahrBis.Visible) Or _
	'				(Me.Cbo_MonatVon.Text <> Me.Cbo_MonatBis.Text And _
	'				 Me.Cbo_MonatBis.Text.Length > 0 And Me.Cbo_MonatBis.Visible)) Then
	'			jobnr += ".G"	' Liste der Arbeitsstunden gruppiert
	'		End If
	'		GetLOAN4Print(False, False, jobnr)
	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}", ex.ToString))
	'	End Try


	'End Sub
	'Private Sub mnuDesignListe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDesignListe.Click
	'	Try
	'		GetLOAN4Print(True, False, "11.5") ' Liste der Arbeitsstunden (Design)
	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}", ex.ToString))
	'	End Try

	'End Sub

	'Private Sub mnuDesignListeGruppiert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDesignListeGruppiert.Click
	'	Try
	'		GetLOAN4Print(True, False, "11.5.G") ' Liste der Arbeitsstunden gruppiert (Design)
	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}", ex.ToString))
	'	End Try

	'End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick

		Try

			FormIsLoaded(frmMyLVName, True)

			m_SearchCriteria = GetSearchKrieteria()
			If Not (Kontrolle()) Then Exit Sub
			Me.txt_SQLQuery.Text = String.Empty


			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			GetMyQueryString()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		Finally

		End Try

	End Sub

	Function Kontrolle() As Boolean
		Try
			Dim msg As String = ""

			Dim monatBis As Integer? = m_SearchCriteria.LastMonth
			Dim jahrBis As Integer? = m_SearchCriteria.LastYear

			If lueMandant.EditValue Is Nothing Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben keinen Mandanten ausgewählt.{0}"), vbNewLine)
			End If

			If Not String.IsNullOrWhiteSpace(msg) Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien unvollständig:{0}{1}"), vbNewLine, msg)
				m_UtilityUI.ShowErrorDialog(msg)

				Return False
			End If

			If Not monatBis.HasValue Then monatBis = m_SearchCriteria.FirstMonth
			If Not jahrBis.HasValue Then jahrBis = m_SearchCriteria.FirstYear

			Dim dtVon As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear))
			Dim dtBis As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", monatBis, jahrBis))
			If DateDiff(DateInterval.Day, dtVon, dtBis) < 0 Then
				msg += String.Format(m_Translate.GetSafeTranslationValue("Das Datum Von kann nicht kleiner als Datum Bis sein.{0}"), vbLf)
			End If

			If Me.txt_MANr.Text.Trim.Length > 0 Then
				For Each manr As String In Me.txt_MANr.Text.Split(CChar(","))
					If Not IsNumeric(manr) Then
						msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist nicht numerisch.{1}"), manr, vbLf)
					ElseIf CInt(manr).ToString <> manr Then
						msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist ungültig.{1}"), manr, vbLf)
					End If
				Next
			End If

			If msg.Length > 0 Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien unvollständig:{0}{1}"), vbLf, msg))

				Return False
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

		Return True
	End Function

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		result.listname = m_Translate.GetSafeTranslationValue("Aufstellung über geleisteten Arbeitsstunden")
		result.mandantenname = lueMandant.Text

		If Me.Cbo_MonatVon.EditValue Is Nothing Then Me.Cbo_MonatVon.EditValue = Now.Month
		If Me.Cbo_JahrVon.EditValue Is Nothing Then Me.Cbo_JahrVon.EditValue = Now.Year

		Dim bismonat As Integer?
		Dim bisjahr As Integer?

		If Not Me.Cbo_MonatBis.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(Me.Cbo_MonatBis.EditValue) Then
			bismonat = CType(Val(Me.Cbo_MonatBis.EditValue), Integer?)
		End If
		If Not Me.Cbo_JahrBis.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(Me.Cbo_JahrBis.EditValue) Then
			bisjahr = CType(Val(Me.Cbo_JahrBis.EditValue), Integer?)
		End If


		result.FirstMonth = Cbo_MonatVon.EditValue
		result.FirstYear = Cbo_JahrVon.EditValue
		If bismonat.HasValue Then result.LastMonth = bismonat Else result.LastMonth = result.FirstMonth
		If bisjahr.HasValue Then result.LastYear = bisjahr Else result.LastYear = result.FirstYear

		result.EmployeeNumbers = txt_MANr.EditValue
		result.Kanton = Cbo_Kanton.EditValue
		result.filiale = Cbo_Filiale.EditValue
		result.Gavberuf = Cbo_Beruf.EditValue

		m_SearchCriteria.sqlsearchstring = String.Empty


		Return result

	End Function

	Function GetMyQueryString() As Boolean

		Try
			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.WorkerReportsProgress = True
			BackgroundWorker1.RunWorkerAsync()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			Return False

		End Try

	End Function


#End Region


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


#Region "Sonstige Funktionen..."

	Private Function LV_GetItemIndex(ByRef lv As ListView) As Integer

		Try
			If lv.Items.Count > 0 Then
				Dim lvi As ListViewItem = lv.SelectedItems(0)		 '.Item(0)
				If lvi.Selected Then
					Return lvi.Index
				Else
					Return -1
				End If
			End If

		Catch ex As Exception

		End Try

	End Function

#End Region


	Private Sub Cbo_Kanton_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Kanton.QueryPopUp
		Try
			Cbo_Kanton.Properties.Items.Clear()
			Dim canton = ListKanton(Me.Cbo_JahrVon.EditValue, Me.Cbo_JahrBis.EditValue, Me.Cbo_MonatVon.EditValue, Me.Cbo_MonatBis.EditValue, Me.Cbo_Beruf.EditValue, Me.txt_MANr.EditValue)
			If (canton Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kantone konnten nicht geladen werden."))
			End If

			If Not (canton Is Nothing) Then
				For i As Integer = 0 To canton.Count - 1
					Cbo_Kanton.Properties.Items.Add(canton(i).ComboValue)
				Next
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))
		End Try

	End Sub

	Private Sub ListBeruf_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Beruf.QueryPopUp

		Try
			Cbo_Beruf.Properties.Items.Clear()
			Dim beruf = ListBeruf(Me.Cbo_JahrVon.EditValue, Me.Cbo_JahrBis.EditValue, Me.Cbo_MonatVon.EditValue, Me.Cbo_MonatBis.EditValue, Me.Cbo_Kanton.EditValue, Me.txt_MANr.EditValue)
			If (beruf Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Berufe konnten nicht geladen werden."))
			End If

			If Not (beruf Is Nothing) Then
				For i As Integer = 0 To beruf.Count - 1
					Cbo_Beruf.Properties.Items.Add(beruf(i).ComboValue)
				Next
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))
		End Try

	End Sub


	''' <summary>
	''' Selektionsfenster für die erste Mitarbeiter-Nummer öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub txt_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANr.ButtonClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim frmTest As New frmSearchRec("MANr")
		frmTest.SelectedYear = Cbo_JahrVon.EditValue

		Dim m As String

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iListeValue()
		Me.txt_MANr.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub OnCbo_Periode_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Periode.QueryPopUp
		Dim datum As Date = Date.Now
		Me.Cbo_Periode.Properties.Items.Clear()

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Diesen Monat ({0:00}/{1})"), datum.Month, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0:00}/{1})"), _
																												datum.AddMonths(-1).Month, datum.AddMonths(-1).Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 1, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 2, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 3, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 4, datum.Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letztes Jahr ({0})"), _
																												datum.AddYears(-1).Year))
	End Sub

	Private Sub Cbo_Periode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_Periode.SelectedIndexChanged
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim dauer As Integer = 0
			Dim datum As Date = Date.Now
			Dim index1 As Integer = datum.Month - 1
			Dim bSetSelectedDate As Boolean = True
			Me.Cbo_MonatBis.Visible = False
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
				Me.Cbo_JahrBis.EditValue = datum.Year

				Me.SwitchButton2.Value = True
				Me.Cbo_MonatVon.EditValue = index1 + 1
				Me.Cbo_MonatBis.EditValue = index1 + 1 + dauer
				Me.Cbo_MonatBis.Visible = True

			Else
				FillDefaultDates()

			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))
		End Try

	End Sub



#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, _
																			 ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		CheckForIllegalCrossThreadCalls = False
		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		Dim i As Integer = 0
		Dim cForeColor As New System.Drawing.Color

		Me.bbiSearch.Enabled = False

		Try
			Dim sSqlQuerySelect As String = String.Empty

			Dim _ClsDb As New ClsDbFunc(m_SearchCriteria)
			Me.txt_SQLQuery.Text = _ClsDb.GetQuerySQLString()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		Finally
			e.Result = True
			If bw.CancellationPending Then e.Cancel = True

		End Try

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, _
																								ByVal e As System.ComponentModel.ProgressChangedEventArgs) _
																								Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, _
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
					Me.Select()
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
					'CreatePrintPopupMenu()
					CreateExportPopupMenu()
				End If

			End If
		End If
		Me.bbiSearch.Enabled = True
		Me.bbiClearFields.Enabled = True

	End Sub

#End Region


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		StartPrinting()
		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

		'If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	'Private Sub CreatePrintPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {"Liste drucken#PrintList", _
	'																				 "Entwurfsansicht#PrintDesign"}
	'	Try
	'		bbiPrint.Manager = Me.BarManager1
	'		BarManager1.ForceInitialize()

	'		Me.bbiPrint.ActAsDropDown = False
	'		Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
	'		Me.bbiPrint.DropDownEnabled = True
	'		Me.bbiPrint.DropDownControl = popupMenu
	'		Me.bbiPrint.Enabled = True

	'		For i As Integer = 0 To liMnu.Count - 1
	'			Dim myValue As String() = liMnu(i).Split(CChar("#"))

	'			If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then bshowMnu = IsUserActionAllowed(0, 560) Else bshowMnu = myValue(0).ToString <> String.Empty
	'			If bshowMnu Then
	'				popupMenu.Manager = BarManager1

	'				Dim itm As New DevExpress.XtraBars.BarButtonItem
	'				itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
	'				itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

	'				If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
	'				AddHandler itm.ItemClick, AddressOf GetMenuItem
	'			End If

	'		Next

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'End Sub

	'Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	Me.PrintJobNr = GetJobID()
	'	Me.SQL4Print = Me.txt_SQLQuery.Text

	'	Try
	'		Select Case e.Item.Name.ToUpper
	'			Case "PrintList".ToUpper
	'				Me.bPrintAsDesign = False

	'			Case "printdesign".ToUpper
	'				Me.bPrintAsDesign = True

	'			Case Else
	'				Exit Sub

	'		End Select
	'		StartPrinting()

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

	'	End Try

	'End Sub

	Sub StartPrinting()

		Dim strFilter As String = String.Empty

		bPrintAsDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 561, lueMandant.EditValue) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Me.PrintJobNr = GetJobID()
		Me.SQL4Print = Me.txt_SQLQuery.Text

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname) '& vbNewLine

		If Not m_SearchCriteria.EmployeeNumbers Is Nothing Then
			strFilter &= If(m_SearchCriteria.EmployeeNumbers.Length > 0, String.Format("{1}Kandidaten: {0}", m_SearchCriteria.EmployeeNumbers, vbNewLine), String.Empty)
		End If
		strFilter &= If(m_SearchCriteria.FirstMonth > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.FirstMonth, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.FirstYear > 0, String.Format(" / {0}", m_SearchCriteria.FirstYear, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.LastMonth <> m_SearchCriteria.FirstMonth, String.Format(" - {0}", m_SearchCriteria.LastMonth), String.Empty)
		strFilter &= If(m_SearchCriteria.FirstYear <> m_SearchCriteria.LastYear, String.Format(" / {0}", m_SearchCriteria.LastYear), String.Empty)

		If Not m_SearchCriteria.filiale Is Nothing Then
			strFilter &= If(m_SearchCriteria.filiale.Length > 0, String.Format("{1}Filiale: {0}", m_SearchCriteria.filiale, vbNewLine), String.Empty)
		End If
		If Not m_SearchCriteria.Kanton Is Nothing Then
			strFilter &= If(m_SearchCriteria.Kanton.Length > 0, String.Format("{1}Kanton: {0}", m_SearchCriteria.Kanton, vbNewLine), String.Empty)
		End If
		If Not m_SearchCriteria.Gavberuf Is Nothing Then
			strFilter &= If(m_SearchCriteria.Gavberuf.Length > 0, String.Format("{1}GAV-Beruf: {0}", m_SearchCriteria.Gavberuf, vbNewLine), String.Empty)
		End If


		Dim _Setting As New ClsLLLOStdSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																												 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																												 .LogedUSNr = m_InitializationData.UserData.UserNr,
																													.SelectedMDYear = m_SearchCriteria.FirstYear,
																													 .SQL2Open = Me.SQL4Print,
																													 .JobNr2Print = Me.PrintJobNr,
																													 .ListBez2Print = m_SearchCriteria.listname,
																													 .frmhwnd = GetHwnd,
																													 .ShowAsDesign = Me.bPrintAsDesign,
																													 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)})
																													}
		Dim obj As New LOStdSearchListing.ClsPrintLOStdSearchList(_Setting)

		obj.PrintLOStdSearchList()

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
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn, _
																																			 .SQL2Open = Me.txt_SQLQuery.Text, _
																																			 .ModulName = "LOStdListToCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromLOStdListing(Me.txt_SQLQuery.Text)

	End Sub


#Region "Funktionen für SwitchButton..."

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.Cbo_MonatBis.Visible = Me.SwitchButton1.Value
		Me.Cbo_MonatBis.EditValue = Nothing
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.Cbo_JahrBis.Visible = Me.SwitchButton2.Value
		Me.Cbo_JahrBis.EditValue = Nothing
	End Sub

#End Region


#Region "Helpers"



#End Region


End Class

