
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


Public Class frmLOLRJSearch
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

	Private ReadOnly Property GetJobID() As String
		Get
			Return "9.6"
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


  
	'#Region "Delegate Variabeln-Deklaration"
	'  ' Der Thread im Hintergrund
	'  Private _ThreadMain As Threading.Thread

	'  ' Das Object (z.B. diese Maske) muss benachrigt werden
	'  Private _SynchronizingObject As System.ComponentModel.ISynchronizeInvoke

	'  ' Das andere Prozess muss benachrichtigt werden, wenn die Suche abgebrochen werden soll.
	'  Private _SynchronizingLV As System.ComponentModel.ISynchronizeInvoke

	'  ' Darin wird die Methode gespeichert, die vom Thread via Delegate aufgerufen werden soll.
	'  Private _NotifyMainProgressDelegate As NotifyMainProgressDel

	'  ' Diese Maske stellt ein Delegate für Benarichtigung des Fortschritts zur Verfügung
	'  Public Delegate Sub NotifyMainProgressDel(ByVal Message As String, ByVal PercentComplete As Integer)

	'  ' Diese Maske muss wissen, wann der nächste Prozess gestartet werden soll
	'  Private _NotifyMainStartLVDelegate As NotifyMainStartLVDel
	'  Public Delegate Sub NotifyMainStartLVDel(ByVal Message As String)

	'  ' Entsprechend muss das Hauptfenster wissen, wenn der nächste Prozess abgeschlossen ist
	'  Private _NotifyMainLVCompletedDelegate As NotifyMainLVCompletedDel
	'  Public Delegate Sub NotifyMainLVCompletedDel(ByVal Message As String)

	'  ' Das Resultat-Fenster muss benachrichtigt werden, wenn das Hauptfenster sich bewegt hat.
	'  Private _NotifyLVMoveDelegate As frmLOLRJSearch_LV.NotifyLVMoveDel
	'#End Region


	' ''' <summary>
	' ''' Diese Methode darf vom Thread aufgerufen werden, wenn die Statusbar verändert werden soll.
	' ''' Sie gibt der Maske sozusagen bescheid. Der Thread hat auf der Maske keine Rechnte mehr.
	' ''' </summary>
	' ''' <param name="Message"></param>
	' ''' <param name="Value"></param>
	' ''' <remarks></remarks>
	'Public Sub NotifyMainProgressBar(ByVal Message As String, ByVal Value As Integer)
	'  Try
	'    If Not _NotifyMainProgressDelegate Is Nothing Then
	'      Dim args(1) As Object
	'      args(0) = Message
	'      args(1) = Value
	'      _SynchronizingObject.Invoke(_NotifyMainProgressDelegate, args)
	'    End If

	'Catch ex As ObjectDisposedException
	'	' Das Objekt wurde zerstört --> keine Fehlermeldung
	'  Catch ex As InvalidOperationException
	'    ' Das Delegate wurde nicht erstellt --> keine Fehlermeldung
	'  Catch ex As Exception ' Manager

	'End Try

	'End Sub

	' ''' <summary>
	' ''' Diese Methode wird ausgeführt, wenn der Thread darum bittet.
	' ''' Das wurde dem Delegate mitgeteilt, welche Methode der Thread ankicken darf.
	' ''' </summary>
	' ''' <param name="Message"></param>
	' ''' <param name="PercentComplete"></param>
	' ''' <remarks></remarks>
	'Private Sub DelegateProgress(ByVal Message As String, ByVal PercentComplete As Integer)
	'  Try
	'    LblState_1.Text = Message
	'    StatusStrip1.Update()
	'  Catch ex As Exception ' Manager

	'End Try

	'End Sub

	' ''' <summary>
	' ''' Diese Methode darf vom Thread aufgerufen werden, wenn die ListView gefüllt werden soll.
	' ''' </summary>
	' ''' <param name="Message"></param>
	' ''' <remarks></remarks>
	'Public Sub NotifyStartLV(ByVal Message As String)
	'  Try
	'    If Not _NotifyMainStartLVDelegate Is Nothing Then
	'      Dim args(0) As Object
	'      args(0) = Message
	'      _SynchronizingObject.Invoke(_NotifyMainStartLVDelegate, args)
	'    End If
	'  Catch ex As ObjectDisposedException
	'    ' Das Objekt wurde zerstört --> keine Fehlermeldung
	'  Catch ex As InvalidOperationException
	'    ' Das Delegate wurde nicht erstellt --> keine Fehlermeldung
	'  Catch ex As Exception ' Manager

	'End Try
	'End Sub


	' ''' <summary>
	' ''' Wenn der separate Thread im LV die Daten übertragen hat, so muss
	' ''' es das Hauptfenster wissen, damit die Suche freigegeben wird und
	' ''' die Anzahl gefundene Datensätze angezeigt wird, usw.
	' ''' </summary>
	' ''' <param name="Message"></param>
	' ''' <remarks></remarks>
	'Public Sub DelegateLVCompleted(ByVal Message As String)
	'  Try
	'    lblwaitpic.Visible = False
	'    LblState_1.Text = String.Format(TranslateMyText("{0} Datensätze angezeigt"), frmMyLV.LvFoundedrecs.Items.Count)
	'    btnSearch.Text = TranslateMyText("Suchen")
	'    ' Die Buttons Drucken und Export aktivieren
	'    Me.btnPrint.Enabled = frmMyLV.LvFoundedrecs.Items.Count > 0
	'    Me.btnExport.Enabled = frmMyLV.LvFoundedrecs.Items.Count > 0

	'    Me.Select()
	'    SuchStatus = SuchStatusEnum.Abgebrochen ' Zustand zurücksetzen

	'  Catch ex As Exception ' Manager

	'End Try
	'End Sub

	' ''' <summary>
	' ''' Während der Datenübertragung zwischen Station und SQL-Server darf nicht unterbrochen werden,
	' ''' da andernfalls die Connection offen bleibt.
	' ''' Grund: Der Thread wird eben nicht unmittelbar beendet und somit kann auch kein Connection.Close()
	' ''' durchgeführt werden bzw. es bleibt so lange offen.
	' ''' </summary>
	' ''' <param name="Abort"></param>
	' ''' <remarks></remarks>
	'Private Sub DelegateAllowAbort(ByVal Abort As Boolean)
	'  _abortDeniedPending = Not Abort
	'  btnSearch.Enabled = Abort

	'  'Close MessageBox programmatically gives you a lot of work. Never mind...A.Ragusa 01.11.2010
	'  ' z.B. --> http://p2p.wrox.com/vb-how/13948-programmatically-close-message-box.html

	'End Sub

	'Enum SuchStatusEnum As Integer
	'  Suchend
	'  Abgebrochen
	'End Enum

	'Dim _suchStatus As SuchStatusEnum = SuchStatusEnum.Abgebrochen
	'Public Property SuchStatus() As SuchStatusEnum
	'  Get
	'    Return _suchStatus
	'  End Get
	'  Set(ByVal value As SuchStatusEnum)
	'    _suchStatus = value
	'  End Set
	'End Property


	'#Region "Dropdown Funktionen Allgemein"


	'	Private Sub ListLOJahr_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'		Try
	'			If DirectCast(sender, myCbo).Items.Count = 0 Then ListLOJahr(sender)
	'		Catch ex As Exception	' Manager
	'			_ClsErrException.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "ListLOJahr_DropDown", ex)
	'		End Try

	'	End Sub

	'#End Region

	'#Region "Allgemeine Funktionen"

	'  ' Monate 1 bis 12
	'  Private Sub Monate1bis12(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'    If TypeOf (sender) Is myCbo Then
	'      Dim cbo As myCbo = DirectCast(sender, myCbo)
	'      If cbo.Items.Count = 0 Then ListCboMonate1Bis12(cbo)
	'    End If
	'  End Sub
	'#End Region


	' ''' <summary>
	' ''' Daten fürs Drucken bereit stellen.
	' ''' </summary>
	' ''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	' ''' <param name="bForExport">ob die Liste für Export ist.</param>
	' ''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	' ''' <remarks></remarks>
	'Sub Get4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
	'  Dim iESNr As Integer = 0
	'  Dim bResult As Boolean = True
	'  Dim storedProc As String = ""
	'  Me.StatusStrip1.Items("lblWaitpic").Visible = True
	'  Me.Cursor = Cursors.WaitCursor

	'  Dim sSql As String = Me.txt_SQLQuery.Text
	'  If sSql = String.Empty Then
	'    MsgBox(TranslateMyText("Keine Suche wurde gestartet!"), MsgBoxStyle.Exclamation, "Get4Print")
	'    Exit Sub
	'  End If

	'  Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
	'  Dim cmd As New SqlCommand(String.Format("SELECT * FROM {0}", ClsDataDetail.LLTablename), Conn)


	'  Try
	'    Conn.Open()


	'    If bForDesign Then
	'      sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 10 ", 1, 1)
	'    Else
	'      ' Für die Fortschrittsanzeige im LL
	'      ClsDataDetail.AnzMax = 0
	'      Dim countReader As SqlDataReader = cmd.ExecuteReader
	'      While countReader.Read
	'        ClsDataDetail.AnzMax += 1
	'      End While
	'      countReader.Close()
	'    End If


	'    Dim rQSTListerec As SqlDataReader = cmd.ExecuteReader                  ' 
	'    Try
	'      If Not rQSTListerec.HasRows Then
	'        cmd.Dispose()
	'        rQSTListerec.Close()

	'        MessageBox.Show(TranslateMyText("Ich konnte leider Keine Daten finden."), _
	'                        "Get4Print", MessageBoxButtons.OK, MessageBoxIcon.Information)
	'      End If

	'    Catch ex As Exception
	'      MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Get4Print")

	'    End Try

	'    While rQSTListerec.Read

	'      'If LL.Core.Handle <> 0 Then LL.Core.LlJobClose()
	'      'LL.Core.LlJobOpen(2)

	'      If bForExport Then
	'        '            ExportLLDoc(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex),CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
	'        '          bResult = ExportLLDocWithStorage(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex), CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
	'      Else
	'        If bForDesign Then
	'				'ShowInDesign(LL, strJobInfo, rQSTListerec)

	'        Else
	'				'bResult = ShowInPrint(LL, strJobInfo, rQSTListerec)

	'        End If
	'      End If

	'      If Not bResult Or bForDesign Then Exit While
	'    End While
	'    rQSTListerec.Close()

	'  Catch ex As Exception ' Manager

	'Finally
	'	cmd.Dispose()
	'	Conn.Close()
	'	Me.StatusStrip1.Items("lblWaitpic").Visible = True
	'	Me.Cursor = Cursors.Default
	'  End Try
	'End Sub


	'Private Sub mnuListeDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuListeDrucken.Click
	'	Get4Print(False, False, "9.6") ' Lohnarten-Rekapitulation
	'End Sub
	'Private Sub mnuDesignListe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDesignListe.Click
	'	Get4Print(True, False, "9.6")	' Lohnarten-Rekapitulation (Design)
	'End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		'Dim Stopwatch As Stopwatch = New Stopwatch()
		'Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		'Dim strSort As String = String.Empty
		'Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)
		'Me.Cursor = Cursors.WaitCursor

		Try

			FormIsLoaded(frmMyLVName, True)

			m_SearchCriteria = GetSearchKrieteria()
			If Not (Kontrolle()) Then Exit Sub
			Me.txt_SQLQuery.Text = String.Empty


			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			GetMyQueryString()


			'Me.StatusStrip1.Update()

			'If SuchStatus = SuchStatusEnum.Abgebrochen Then
			'	FormIsLoaded(frmMyLVName, True)
			'End If

			'' PROGRESSBAR
			'If _SynchronizingObject Is Nothing Then
			'	_SynchronizingObject = Me
			'	_NotifyMainProgressDelegate = New NotifyMainProgressDel(AddressOf DelegateProgress)
			'	_NotifyMainStartLVDelegate = New NotifyMainStartLVDel(AddressOf ResultatFensterAnzeigen)
			'End If


			'If SuchStatus = SuchStatusEnum.Abgebrochen Then
			'	SuchStatus = SuchStatusEnum.Suchend
			'	_ThreadMain = New Threading.Thread(AddressOf SucheStarten)
			'	_ThreadMain.Name = "LohnkontiSuche"
			'	_ThreadMain.IsBackground = True	' So wird nicht verhindert, dass die Maske so lange offen bleibt, wie dieser Thread läuft.
			'	_ThreadMain.Start()
			'	btnSearch.Text = TranslateMyText("Abbrechen")
			'	lblWaitpic.Visible = True
			'Else
			'	If Not _abortDeniedPending Then
			'		btnSearch.Enabled = False
			'	End If
			'	If MessageBox.Show(TranslateMyText("Wollen Sie die Suche wirklich abbrechen?"), _
			'										 TranslateMyText("Suche abbrechen..."), _
			'											 MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
			'		If SuchStatus = SuchStatusEnum.Suchend Then	' Kann ja sein, das der Thread die Resultate in der Zwischenzeit geliefert hat.
			'			_ThreadMain.Abort()
			'			If Not frmMyLV Is Nothing Then
			'				frmMyLV.NotifyLVAbortThread("")
			'			End If
			'			SuchStatus = SuchStatusEnum.Abgebrochen
			'			btnSearch.Text = TranslateMyText("Suchen")
			'			lblWaitpic.Visible = False
			'			NotifyMainProgressBar(TranslateMyText("Suche abgebrochen"), 0)
			'		End If
			'	End If
			'	If Not _abortDeniedPending Then
			'		btnSearch.Enabled = True
			'	End If
			'End If

		Catch ex As Exception	' Manager

		Finally

		End Try

	End Sub

	' ''' <summary>
	' ''' Diese Methode wird vom Thread aufgerufen und hat auf der Maske keine Rechte mehr.
	' ''' Jegliche von dieser Methode folgende Aufrufe laufen im diesem Thread und werden
	' ''' geschlossen, so bald der Thread beendet wird. Rechte haben diese übrigens ebenso keine.
	' ''' </summary>
	' ''' <remarks></remarks>
	'Private Sub SucheStarten()
	'	Dim Stopwatch As Stopwatch = New Stopwatch()
	'	Dim sSql1Query As String = String.Empty
	'	Dim sSql2Query As String = String.Empty
	'	Dim strSort As String = String.Empty
	'	Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)

	'	Try
	'		NotifyMainProgressBar(TranslateMyText("Die Query wird zusammengestellt"), 1)
	'		' Die Query-String aufbauen...
	'		GetMyQueryString(Me)
	'		NotifyMainProgressBar(TranslateMyText("Die Query wird zusammengestellt"), 90)

	'		Stopwatch.Reset()
	'		Stopwatch.Start()
	'		NotifyMainProgressBar(TranslateMyText("Suche abgeschlossen"), 100)
	'		NotifyStartLV("ListView")
	'	Catch ex As Threading.ThreadAbortException
	'		' Der Thread wurde abgebrochen --> Kein Fehler
	'	Catch ex As Exception	' Manager
	'		_ClsErrException.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "SucheStarten", ex)
	'	End Try
	'End Sub

	'Private Sub ResultatFensterAnzeigen()
	'	Try
	'		NotifyMainProgressBar(TranslateMyText("Daten werden aufbereitet"), 1)
	'		If Not FormIsLoaded(frmMyLVName, True) Then
	'			frmMyLV = New frmLOLRJSearch_LV(Me.Location.X, Me.Location.Y, Me.Height, Me, _
	'												New frmLOLRJSearch_LV.NotifyMainProgressBarDel(AddressOf DelegateProgress), _
	'												New frmLOLRJSearch_LV.NotifyMainLVCompletedDel(AddressOf DelegateLVCompleted), _
	'												New frmLOLRJSearch_LV.NotifyMainAllowAbortDel(AddressOf DelegateAllowAbort))

	'			frmMyLV.Show()
	'		End If
	'		NotifyMainProgressBar(TranslateMyText("Daten Aufbereitung abgeschlossen"), 100)
	'	Catch ex As Exception	' Manager
	'		_ClsErrException.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "ResultatFensterAnzeigen", ex)
	'	End Try

	'End Sub



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


		result.listname = m_Translate.GetSafeTranslationValue("Jährliche Lohnrekapitulation")
		result.mandantenname = lueMandant.Text

		If Me.Cbo_JahrVon.EditValue Is Nothing Then Me.Cbo_JahrVon.EditValue = Now.Year

		result.FirstYear = Cbo_JahrVon.EditValue

		m_SearchCriteria.sqlsearchstring = String.Empty

		Return result

	End Function


	Function GetMyQueryString() As Boolean

		Try

			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.WorkerReportsProgress = True
			BackgroundWorker1.RunWorkerAsync()

			'Dim sSqlQuerySelect As String = String.Empty


			'_ClsDb._SynchronizingMain = Me
			'_ClsDb._SynchronizingThread = _ThreadMain
			'_ClsDb._NotifyMainProgressDelegate = New ClsDbFunc.NotifyMainProgressDel(AddressOf DelegateProgress)
			'_ClsDb._NotifyMainAllowAbortDelegate = New ClsDbFunc.NotifyMainAllowAbortDel(AddressOf DelegateAllowAbort)

			'If Me.txt_IndSQLQuery.Text = String.Empty Then
			'  ' Selektion als Parameter
			'Me.txt_SQLQuery.Text = _ClsDb.GetQuerySQLString(frm)
			'   Else
			'Me.txt_SQLQuery.Text = Me.txt_IndSQLQuery.Text
			'   End If
			'NotifyMainProgressBar("QueryString beendet", 80)

		Catch ex As Threading.ThreadAbortException
			' Der Thread wurde abgebrochen --> Kein Fehler
		Catch ex As Exception	' Manager
			'_ClsErrException.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "GetMyQueryString", ex)
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

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, _
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
			Me.txt_SQLQuery.Text = _ClsDb.GetQuerySQLString()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		Finally
			e.Result = True
			If bw.CancellationPending Then e.Cancel = True
			Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

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
					'CreatePrintPopupMenu()
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
																																			 .ModulName = "JLORekapListToCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromJLORekapListing(Me.txt_SQLQuery.Text)

	End Sub




#Region "Helpers"

	Sub StartPrinting()
		Dim strFilter As String = String.Empty

		bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)
		Me.PrintJobNr = GetJobID()
		Me.SQL4Print = Me.txt_SQLQuery.Text

		strFilter &= String.Format("Mandant: {0}{1}", m_SearchCriteria.mandantenname, vbNewLine)
		strFilter &= If(m_SearchCriteria.FirstYear > 0, String.Format("Jahr: {0}{1}", m_SearchCriteria.FirstYear, vbNewLine), String.Empty)

		Dim _Setting As New ClsLLJLohnRekapSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
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
		Dim obj As New JLohnRekapSearchListing.ClsPrintJLohnRekapSearchList(_Setting)

		obj.PrintJLohnRekapSearchList()

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
