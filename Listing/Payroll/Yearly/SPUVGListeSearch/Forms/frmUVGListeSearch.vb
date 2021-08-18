
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
Imports SPProgUtility.ProgPath

Public Class frmUVGListeSearch

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

		AddHandler Cbo_JahrBis.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_BMonth_1.ButtonClick, AddressOf OnDropDown_ButtonClick

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
			Return If(Art = 0, "All", If(Art = 2, "9.5", "9.7"))
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
		Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)

		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabSQLQuery.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQLQuery.Text)

		Me.beiWorking.Caption = m_Translate.GetSafeTranslationValue(Me.beiWorking.Caption)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClearFields.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClearFields.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

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

		If e.Button.Index = 1 Then
			Dim cbo As ComboBoxEdit = CType(sender, ComboBoxEdit)
			cbo.EditValue = Nothing

			Return
		End If

		Dim frmTest As New frmSearchRec("MANr")
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
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0:00}/{1})"), datum.AddMonths(-1).Month, datum.AddMonths(-1).Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 1, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 2, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 3, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 4, datum.Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Dieses Jahr ({0})"), datum.AddYears(0).Year))
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

				Case 7 ' "LJ"
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

	Private Sub ListLOJahr_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_JahrVon.QueryPopUp
		ListLOJahr(sender)
	End Sub

	Private Sub ListLOMonat_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VMonth_1.QueryPopUp, Cbo_BMonth_1.QueryPopUp
		ListMonth(sender, Cbo_JahrVon.EditValue)
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

			ListLOJahr(Me.Cbo_JahrVon)
			ListLOJahr(Me.Cbo_JahrBis)

			' Bis am 10. des Monats wird der Vormonat selektiert. Ab 11. den aktuellen Monat
			Dim datum As Date = Date.Now
			If datum.Day < 11 Then
				datum = datum.AddMonths(-1)
			End If
			Me.Cbo_JahrVon.EditValue = datum.Year
			Me.Cbo_JahrBis.EditValue = Nothing

			ListMonth(Me.Cbo_VMonth_1, Cbo_JahrVon.EditValue)
			ListMonth(Me.Cbo_BMonth_1, Cbo_JahrVon.EditValue)

			Me.Cbo_VMonth_1.EditValue = datum.Month
			Me.Cbo_BMonth_1.EditValue = Nothing

			Me.Cbo_VMonth_1.EditValue = datum.Month

		Catch ex As Exception

		End Try

	End Sub


	' Private Sub FillDefaultDates()
	'   Try
	'     ' DATUM ---------------------------------
	'     ' Dropdown von Monat und Jahr müssen vorbelegt sein.
	'     If Me.Cbo_MonatVon.Properties.Items.Count = 0 Then
	'       ListCboMonate1Bis12(Me.Cbo_MonatVon)
	'       ListCboMonate1Bis12(Me.Cbo_MonatBis)
	'       ListLOJahr(Me.Cbo_JahrVon)
	'       ListLOJahr(Me.Cbo_JahrBis)
	'     End If
	'     ' Bis am 10. des Monats wird der Vormonat selektiert. Ab 11. den aktuellen Monat
	'     Dim datum As Date = Date.Now
	'     If datum.Day < 11 Then
	'       datum = datum.AddMonths(-1)
	'     End If
	'     Me.Cbo_MonatVon.Text = datum.Month.ToString
	'     Me.Cbo_MonatBis.Text = ""
	'     Me.Cbo_JahrBis.Text = ""
	'     Me.Cbo_JahrVon.Text = datum.Year
	'     Me.Cbo_MonatVon.Text = datum.Month

	'   Catch ex As Exception ' Manager

	'	End Try

	' End Sub

	' ''' <summary>
	' ''' Default-Werte in den Felder Monat und Jahr einfügen.
	' ''' </summary>
	' ''' <remarks></remarks>
	' Sub FillDefaultValues()
	'   ' Dropdown von Monat und Jahr müssen vorbelegt sein.
	'   ListLOJahr(Me.Cbo_JahrVon)
	'   ListLOJahr(Me.Cbo_JahrBis)
	'   ListCboMonate1Bis12(Me.Cbo_MonatVon)
	'   ListCboMonate1Bis12(Me.Cbo_MonatBis)

	'   Me.Cbo_JahrVon.Enabled = True
	'   Me.Cbo_JahrBis.Enabled = True
	'   Me.Cbo_MonatVon.Enabled = True
	'   Me.Cbo_MonatBis.Enabled = True

	'   If Me.Cbo_MonatVon.Text.Length = 0 Then If Now.Day <= 15 Then Me.Cbo_MonatVon.Text = Now.Month - 1 Else Me.Cbo_MonatVon.Text = Now.Month
	'   If Me.Cbo_JahrVon.Text.Length = 0 Then If Now.Day <= 15 And Now.Month = 1 Then Me.Cbo_MonatVon.Text = Now.Year - 1 Else Me.Cbo_JahrVon.Text = Now.Year

	'   'CheckSearchInputs()

	' End Sub

	' ''' <summary>
	' ''' Daten fürs Drucken bereit stellen.
	' ''' </summary>
	' ''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	' ''' <param name="bForExport">ob die Liste für Export ist.</param>
	' ''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	' ''' <remarks></remarks>
	'Sub GetUVGListeData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
	'  Dim iESNr As Integer = 0
	'  Dim bResult As Boolean = True
	'  Dim storedProc As String = ""

	'  ClsDataDetail.GetModulToPrint = strJobInfo

	'  Dim sSql As String = Me.txt_SQLQuery.Text
	'  If sSql = String.Empty Then
	'    MsgBox(TranslateMyText("Keine Suche wurde gestartet!"), MsgBoxStyle.Exclamation, "GetESData4Print")
	'    Exit Sub
	'  End If

	'  ' Bei Formularanpassung nur die ersten 10 Datensätze holen
	'  If bForDesign Then
	'    If strJobInfo = "9.5" Then ' Die Rekapitulation braucht nur 1 Datensatz
	'      sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 1 ", 1, 1)
	'    Else
	'      sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 10 ", 1, 1)
	'    End If
	'  Else
	'    If strJobInfo = "9.5" Then ' Die Rekapitulation braucht nur 1 Datensatz
	'      ' SELECT * FROM _UVGListe_1 ORDER BY ID 
	'      sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 1 ", 1, 1)
	'    End If
	'  End If

	'  'Daten sind auf der Datenbank
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
	'  Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

	'  Try
	'    Conn.Open()

	'    Dim rUVGListerec As SqlDataReader = cmd.ExecuteReader                  ' 

	'    Try
	'      If Not rUVGListerec.HasRows Then
	'        cmd.Dispose()
	'        rUVGListerec.Close()

	'        MessageBox.Show(TranslateMyText("Ich konnte leider Keine Daten finden."), _
	'                        "GetUVGListeData", MessageBoxButtons.OK, MessageBoxIcon.Information)
	'      End If

	'    Catch ex As Exception
	'      MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetUVGListeData")

	'    End Try
	'    rUVGListerec.Read()
	'    If rUVGListerec.HasRows Then
	'      Me.SQL4Print = sSql
	'      Me.bPrintAsDesign = bForDesign
	'      'Trace.WriteLine(String.Format("Job-Nr: {0}", strJobInfo))


	'      Me.PrintJobNr = strJobInfo

	'      PrintListingThread = New Thread(AddressOf StartPrinting)
	'      PrintListingThread.Name = String.Format("PrintingUVGListing.{0}", strJobInfo)
	'      PrintListingThread.SetApartmentState(ApartmentState.STA)
	'      PrintListingThread.Start()

	'    End If
	'    rUVGListerec.Close()

	'  Catch ex As Exception
	'    MsgBox(ex.Message, MsgBoxStyle.Critical, "GetUVGListeData4Print")

	'  Finally
	'    cmd.Dispose()
	'    Conn.Close()
	'  End Try
	'End Sub


	'Sub StartPrinting()
	'	Dim m_path As New ClsProgPath
	'	Dim strFilter As String = String.Empty

	'	strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname) & vbNewLine

	'	strFilter &= If(m_SearchCriteria.firstMonth > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.firstMonth, vbNewLine), String.Empty)
	'	strFilter &= If(m_SearchCriteria.firstYear > 0, String.Format(" / {0}", m_SearchCriteria.firstYear, vbNewLine), String.Empty)
	'	strFilter &= If(m_SearchCriteria.lastMonth > 0, String.Format(" - {0}", m_SearchCriteria.lastMonth), String.Empty)
	'	strFilter &= If(m_SearchCriteria.lastYear > 0, String.Format(" / {0}", m_SearchCriteria.lastYear), String.Empty)

	'	strFilter &= If(Not m_SearchCriteria.MANrList Is Nothing AndAlso String.IsNullOrWhiteSpace(m_SearchCriteria.MANrList),
	'									String.Format("{1}Kandidaten: {0}", m_SearchCriteria.MANrList, vbNewLine), String.Empty)

	'	Dim _Setting As New SPS.Listing.Print.Utility.ClsLLUVGSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
	'																																								 .SelectedMDNr = m_InitializationData.MDData.MDNr,
	'																																								 .SelectedMDYear = m_SearchCriteria.firstYear,
	'																																								 .LogedUSNr = m_InitializationData.UserData.UserNr,
	'																																								 .SQL2Open = Me.SQL4Print,
	'																																								 .frmhwnd = GetHwnd,
	'																																								 .ShowAsDesign = Me.bPrintAsDesign,
	'																																								 .JobNr2Print = Me.PrintJobNr,
	'																																								 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)})}
	'	Dim esAnzM As Integer = 0
	'	Dim esAnzF As Integer = 0

	'	' FÜR DIE REKAPITULATION
	'	'Dim conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
	'	'Dim cmd As SqlCommand = New SqlCommand("", conn)
	'	'Dim pJahr As SqlParameter = New SqlParameter("@mdYear", SqlDbType.NVarChar, 4)
	'	'cmd.Parameters.Add(pJahr)
	'	'' ESAnzahlF, ESAnzahlM
	'	'Dim esAnzM As Integer = 0
	'	'Dim esAnzF As Integer = 0
	'	'If ClsDataDetail.LLShowESAnzJahr Then
	'	'	cmd.CommandText = "[Get AnzM In End Of September With Mandant]"
	'	'	cmd.CommandType = CommandType.StoredProcedure
	'	'	pJahr.Value = ClsDataDetail.LLESAnzJahr
	'	'	Dim dtAnzEndeSeptember As DataTable = New DataTable()
	'	'	Dim daAnzEndeSeptember As SqlDataAdapter = New SqlDataAdapter(cmd)
	'	'	daAnzEndeSeptember.Fill(dtAnzEndeSeptember)
	'	'	If dtAnzEndeSeptember.Rows.Count > 0 Then
	'	'		For Each row As DataRow In dtAnzEndeSeptember.Rows
	'	'			If row("Geschlecht").ToString.ToUpper = "M" Then
	'	'				esAnzM += 1
	'	'			ElseIf row("Geschlecht").ToString.ToUpper = "W" Then
	'	'				esAnzF += 1
	'	'			End If
	'	'		Next
	'	'	End If
	'	'End If

	'	Dim EmployeeData = GetEmployeeCountPerSeptember(m_SearchCriteria.firstYear)
	'	If Not EmployeeData Is Nothing Then
	'		esAnzM = EmployeeData.EmployeeMCount
	'		esAnzF = EmployeeData.EmployeeFCount
	'	End If
	'	_Setting.esAnzM = esAnzM
	'	_Setting.esAnzF = esAnzF
	'	_Setting.LLShowESAnzJahr = ClsDataDetail.LLShowESAnzJahr
	'	_Setting.LLESAnzJahr = ClsDataDetail.LLESAnzJahr

	'	_Setting.JahrVon2Print = m_SearchCriteria.firstYear
	'	_Setting.JahrBis2Print = m_SearchCriteria.lastYear
	'	_Setting.MonatVon2Print = m_SearchCriteria.firstMonth
	'	_Setting.MonatBis2Print = m_SearchCriteria.lastMonth


	'	_Setting.TotalSuva.Add(ClsDataDetail.LLTotalMA12)
	'	_Setting.TotalSuva.Add(ClsDataDetail.LLTotalMA3)
	'	_Setting.TotalSuva.Add(ClsDataDetail.LLTotalFA12)
	'	_Setting.TotalSuva.Add(ClsDataDetail.LLTotalFA3)
	'	_Setting.TotalSuva.Add(ClsDataDetail.LLTotalMZ12)
	'	_Setting.TotalSuva.Add(ClsDataDetail.LLTotalMZ3)
	'	_Setting.TotalSuva.Add(ClsDataDetail.LLTotalFZ12)
	'	_Setting.TotalSuva.Add(ClsDataDetail.LLTotalFZ3)


	'	Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}", m_InitializationData.MDData.MDNr)
	'	Dim mdXMLFile As String = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year)

	'	Dim Abrechnungsnummer As String = m_path.GetXMLNodeValue(mdXMLFile, String.Format("{0}/SUVA-Daten/Abrechnungsnummer", FORM_XML_MAIN_KEY))

	'	If String.IsNullOrWhiteSpace(Abrechnungsnummer) Then
	'		m_UtilityUI.ShowErrorDialog(String.Format("Sie haben keine Unfallversicherungsnummer eingetragen." &
	'																	"{0}Bitte tragen Sie die Daten in Ihren Systemkonstanten ein.", vbNewLine))
	'	End If
	'	_Setting.ausgleichsNr = Abrechnungsnummer

	'	_Setting.MDsuva_hl = GetSuvaHL(m_SearchCriteria.firstYear)
	'	'conn.Close()


	'	' Für die Rekapitulation
	'	'If ClsDataDetail.GetModulToPrint = "9.5" Then
	'	For i As Integer = 0 To ClsDataDetail.LLSecSuvaItems.Count - 1
	'		Dim item As ClsDataDetail.SecSuvaItem = DirectCast(ClsDataDetail.LLSecSuvaItems.Item(i), ClsDataDetail.SecSuvaItem)
	'		_Setting.SecSuvaCode.Add(item.Bezeichnung, String.Format("{0}|{1}", item.M, item.F))
	'	Next
	'	' Die restlichen leeren Felder für die Variablen im Formular deklarieren
	'	For i As Integer = ClsDataDetail.LLSecSuvaItems.Count To 6
	'		_Setting.SecSuvaCode.Add(CStr(i), String.Format("{0}|{1}", 0, 0))
	'	Next
	'	'End If

	'	Dim obj As New SPS.Listing.Print.Utility.UVGSearchListing.ClsPrintUVGSearchList(_Setting)
	'	ClsDataDetail.GetFilterBez = String.Empty
	'	ClsDataDetail.GetFilterBez2 = String.Empty
	'	ClsDataDetail.GetFilterBez3 = String.Empty
	'	ClsDataDetail.GetFilterBez4 = String.Empty
	'	_Setting.PrintUVGWithSuvaRekap = Me.PrintJobNr = "All"
	'	obj.PrintUVGSearchList()

	'	'Me.bPrintAsDesign, ClsDataDetail.GetSortBez, _
	'	'											New List(Of String)(New String() {ClsDataDetail.GetFilterBez, _
	'	'																												ClsDataDetail.GetFilterBez2, _
	'	'																												ClsDataDetail.GetFilterBez3, _
	'	'																												ClsDataDetail.GetFilterBez4}))


	'End Sub


#Region "Funktionen zur Menüaufbau..."

	'Private Sub mnuPrintAllLists_Click(sender As System.Object, e As System.EventArgs) Handles mnuPrintAllLists.Click
	'  GetUVGListeData4Print(False, False, "All")
	'End Sub

	'Private Sub mnuUVGListeDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUVGListeDrucken.Click
	'  GetUVGListeData4Print(False, False, "9.7") ' UVG-Liste (Pro MA)
	'End Sub

	'Private Sub mnuUVGRekapitulationDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUVGRekapitulationDrucken.Click
	'  GetUVGListeData4Print(False, False, "9.5") 'UVG-Liste (Rekapitulation)
	'End Sub

	'Private Sub mnuDesignUVGListe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDesignUVGListe.Click
	'  GetUVGListeData4Print(True, False, "9.7") ' UVG-Liste (Pro MA) (Design)
	'End Sub

	'Private Sub mnuDesignRekapitulation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDesignRekapitulation.Click
	'  GetUVGListeData4Print(True, False, "9.5") 'UVG-Liste (Rekapitulation) (Design)
	'End Sub

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

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick


		Try
			bbiClearFields.Enabled = False
			bbiPrint.Enabled = False
			bbiExport.Enabled = False

			FormIsLoaded(frmMyLVName, True)

			m_SearchCriteria = GetSearchKrieteria()
			If Not (Kontrolle()) Then Exit Sub
			Me.txt_SQLQuery.Text = String.Empty


			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			' Die Query-String aufbauen...
			GetMyQueryString()


		Catch ex As Exception

		Finally

		End Try

	End Sub

	Function Kontrolle() As Boolean
		Try
			Dim msg As String = ""

			Dim jahrVon As Integer = m_SearchCriteria.firstYear.GetValueOrDefault(0)
			Dim monthVon As Integer = m_SearchCriteria.firstMonth.GetValueOrDefault(0)

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


		result.listname = m_Translate.GetSafeTranslationValue("UVG-Jahresabrechnung")
		result.mandantenname = lueMandant.Text

		result.MANrList = txt_MANr.EditValue

		Dim lastyear As Integer? = Me.Cbo_JahrBis.EditValue
		If Not lastyear.HasValue Then lastyear = Cbo_JahrVon.EditValue

		Dim lastmonth As Integer? = Me.Cbo_BMonth_1.EditValue
		If Not lastmonth.HasValue Then lastmonth = Cbo_VMonth_1.EditValue

		result.firstYear = Cbo_JahrVon.EditValue
		result.lastYear = lastyear

		result.firstMonth = Cbo_VMonth_1.EditValue
		result.lastMonth = lastmonth

		m_SearchCriteria.sqlsearchstring = String.Empty

		Return result

	End Function


	Function GetMyQueryString() As Boolean

		BackgroundWorker1.WorkerSupportsCancellation = True
		BackgroundWorker1.WorkerReportsProgress = True
		BackgroundWorker1.RunWorkerAsync()

		Return True
	End Function



#End Region



#Region "Multitreading..."


	Private Sub OnBgWorkerDoWork(ByVal sender As System.Object, _
																		 ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		CheckForIllegalCrossThreadCalls = False
		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		Dim i As Integer = 0
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

		Me.bbiSearch.Enabled = False

		Try
			Dim sSqlQuerySelect As String = String.Empty

			Dim _ClsDb As New ClsDbFunc(m_SearchCriteria)
			Dim sSql1Query As String = _ClsDb.GetStartSQLString()
			If String.IsNullOrWhiteSpace(sSql1Query) Then Throw New Exception("Fehler beim Löschen der Daten.")

			'Dim sSql2Query As String = _ClsDb.GetStartSQLString()		 ' Where Klausel
			'Me.txt_SQLQuery.Text = sSql2Query

			Me.txt_SQLQuery.Text = sSql1Query

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
					CreatePrintPopupMenu()
					CreateExportPopupMenu()
				End If

			End If
		End If
		bbiClearFields.Enabled = True
		Me.bbiSearch.Enabled = True
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

	End Sub

#End Region


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Alle Listen zusammendrucken#PrintList", "UVG-Liste drucken#PrintUVGList", "UVG-Rekapitulation-Liste drucken#PrintRekapList"}

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

		Me.SQL4Print = Me.txt_SQLQuery.Text
		Try
			Select Case e.Item.Name.ToUpper
				Case "PrintList".ToUpper
					Me.bPrintAsDesign = False
					Art = 0

				Case "PrintUVGList".ToUpper
					Me.bPrintAsDesign = False
					Art = 1

				Case "PrintRekapList".ToUpper
					Me.SQL4Print = String.Format("SELECT Top 1 * FROM {0} ORDER BY ID ", ClsDataDetail.LLTabellennamen)
					Me.bPrintAsDesign = False
					Art = 2

					'Case "printdesign".ToUpper
					'	Me.bPrintAsDesign = True
					'	Art = 1

					'Case "PrintUVGRekapDesign".ToUpper
					'	Me.SQL4Print = String.Format("SELECT Top 1 * FROM {0} ORDER BY ID ", ClsDataDetail.LLTabellennamen)
					'	Me.bPrintAsDesign = True
					'	Art = 2


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
																																			 .SQL2Open = Me.txt_SQLQuery.Text,
																																			 .ModulName = "UVGListToCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromUVGListing(Me.txt_SQLQuery.Text)

	End Sub

	Sub StartPrinting()
		Dim m_path As New ClsProgPath
		Dim strFilter As String = String.Empty
		bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname)

		strFilter &= If(m_SearchCriteria.firstMonth > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.firstMonth, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.firstYear > 0, String.Format(" / {0}", m_SearchCriteria.firstYear, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.lastMonth > 0, String.Format(" - {0}", m_SearchCriteria.lastMonth), String.Empty)
		strFilter &= If(m_SearchCriteria.lastYear > 0, String.Format(" / {0}", m_SearchCriteria.lastYear), String.Empty)

		strFilter &= If(Not m_SearchCriteria.MANrList Is Nothing AndAlso String.IsNullOrWhiteSpace(m_SearchCriteria.MANrList),
										String.Format("{1}Kandidaten: {0}", m_SearchCriteria.MANrList, vbNewLine), String.Empty)

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLUVGSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																									 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																																									 .SelectedMDYear = m_SearchCriteria.firstYear,
																																									 .LogedUSNr = m_InitializationData.UserData.UserNr,
																																									 .SQL2Open = Me.SQL4Print,
																																									 .frmhwnd = GetHwnd,
																																									 .ShowAsDesign = Me.bPrintAsDesign,
																																									 .JobNr2Print = Me.PrintJobNr,
																																									 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)})}
		Dim esAnzM As Integer = 0
		Dim esAnzF As Integer = 0

		Dim EmployeeData = GetEmployeeCountPerSeptember(m_SearchCriteria.firstYear)
		If Not EmployeeData Is Nothing Then
			esAnzM = EmployeeData.EmployeeMCount
			esAnzF = EmployeeData.EmployeeFCount
		End If
		_Setting.esAnzM = esAnzM
		_Setting.esAnzF = esAnzF
		_Setting.LLShowESAnzJahr = ClsDataDetail.LLShowESAnzJahr
		_Setting.LLESAnzJahr = ClsDataDetail.LLESAnzJahr

		_Setting.JahrVon2Print = m_SearchCriteria.firstYear
		_Setting.JahrBis2Print = m_SearchCriteria.lastYear
		_Setting.MonatVon2Print = m_SearchCriteria.firstMonth
		_Setting.MonatBis2Print = m_SearchCriteria.lastMonth


		_Setting.TotalSuva.Add(ClsDataDetail.LLTotalMA12)
		_Setting.TotalSuva.Add(ClsDataDetail.LLTotalMA3)
		_Setting.TotalSuva.Add(ClsDataDetail.LLTotalFA12)
		_Setting.TotalSuva.Add(ClsDataDetail.LLTotalFA3)
		_Setting.TotalSuva.Add(ClsDataDetail.LLTotalMZ12)
		_Setting.TotalSuva.Add(ClsDataDetail.LLTotalMZ3)
		_Setting.TotalSuva.Add(ClsDataDetail.LLTotalFZ12)
		_Setting.TotalSuva.Add(ClsDataDetail.LLTotalFZ3)

		Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}", m_InitializationData.MDData.MDNr)
		Dim mdXMLFile As String = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year)

		Dim Abrechnungsnummer As String = m_path.GetXMLNodeValue(mdXMLFile, String.Format("{0}/SUVA-Daten/Abrechnungsnummer", FORM_XML_MAIN_KEY))

		If String.IsNullOrWhiteSpace(Abrechnungsnummer) Then
			m_UtilityUI.ShowErrorDialog(String.Format("Sie haben keine Unfallversicherungsnummer eingetragen." &
																		"{0}Bitte tragen Sie die Daten in Ihren Systemkonstanten ein.", vbNewLine))
		End If
		_Setting.ausgleichsNr = Abrechnungsnummer

		_Setting.MDsuva_hl = GetSuvaHL(m_SearchCriteria.firstYear)


		' Für die Rekapitulation
		'If ClsDataDetail.GetModulToPrint = "9.5" Then
		For i As Integer = 0 To ClsDataDetail.LLSecSuvaItems.Count - 1
			Dim item As ClsDataDetail.SecSuvaItem = DirectCast(ClsDataDetail.LLSecSuvaItems.Item(i), ClsDataDetail.SecSuvaItem)
			_Setting.SecSuvaCode.Add(item.Bezeichnung, String.Format("{0}|{1}", item.M, item.F))
		Next
		' Die restlichen leeren Felder für die Variablen im Formular deklarieren
		For i As Integer = ClsDataDetail.LLSecSuvaItems.Count To 6
			_Setting.SecSuvaCode.Add(CStr(i), String.Format("{0}|{1}", 0, 0))
		Next
		'End If

		Dim obj As New SPS.Listing.Print.Utility.UVGSearchListing.ClsPrintUVGSearchList(_Setting)
		ClsDataDetail.GetFilterBez = String.Empty
		ClsDataDetail.GetFilterBez2 = String.Empty
		ClsDataDetail.GetFilterBez3 = String.Empty
		ClsDataDetail.GetFilterBez4 = String.Empty

		_Setting.PrintUVGWithSuvaRekap = Me.PrintJobNr = "All"
		obj.PrintUVGSearchList()


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




#Region "Funktionen für SwitchButton..."

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.Cbo_BMonth_1.Visible = Me.SwitchButton1.Value
		Me.Cbo_BMonth_1.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.Cbo_JahrBis.Visible = Me.SwitchButton2.Value
		Me.Cbo_JahrBis.Text = String.Empty
	End Sub

#End Region

End Class

' ''' <summary>
' ''' Klasse für die ComboBox, um Text und Wert zu haben.
' ''' Das Item wird mit den Parameter Text für die Anzeige und
' ''' Value für den Wert zur ComboBox hinzugefügt.
' ''' </summary>
' ''' <remarks></remarks>
'Class ComboBoxItem
'  Public Text As String
'  Public Value As String
'  Public Sub New(ByVal text As String, ByVal val As String)
'    Me.Text = text
'    Me.Value = val
'  End Sub
'  Public Overrides Function ToString() As String
'    Return Text
'  End Function
'End Class


'Module StringExtensions

'  <Extension()> _
'  Public Sub KommasEntfernen(ByRef text As String)
'    Do While (text.Contains(",,"))
'      text = text.Replace(",,", ",")
'    Loop
'    If text.StartsWith(",") Then
'      text = text.Remove(0, 1)
'    End If
'    If text.EndsWith(",") Then
'      text = text.Remove(text.Length - 1, 1)
'    End If
'  End Sub

'End Module
