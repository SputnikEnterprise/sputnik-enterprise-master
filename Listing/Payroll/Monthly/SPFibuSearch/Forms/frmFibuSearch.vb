
Option Strict Off

Imports SP.Infrastructure.Logging
Imports System.Reflection.Assembly
Imports System.Data.SqlClient

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports System.Threading
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.LookAndFeel

Imports SP.Infrastructure.UI

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPFibuSearch.ClsDataDetail
Imports SPS.Listing.Print.Utility
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Public Class frmFibuSearch
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_ConnectionString As String

	''' <summary>
	''' The data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	Private _ClsFunc As New ClsDivFunc

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Public Shared frmMyLV As frmFibuSearch_LV

	Private Property ShortSQLQuery As String
	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Dim PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintedPages As Boolean


	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As New SearchCriteria


#Region "Contructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)


		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		ResetMandantenDropDown()
		LoadMandantenDropDown()


	End Sub

#End Region


#Region "Lookup Edit Reset und Load..."

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
		Dim SelectedData As MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, ClsDataDetail.m_InitialData.UserData.UserNr)
			m_InitializationData = ClsDataDetail.m_InitialData

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)

	End Sub


#End Region


#Region "Dropdown Funktionen 1. Seite..."

	Private Sub Cbo_Filiale_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		If Me.Cbo_Filiale.Properties.Items.Count = 0 Then ListUJFiliale(Me.Cbo_Filiale)
	End Sub

	Private Sub Cbo_Month_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Month.QueryPopUp
		If Me.Cbo_Month.Properties.Items.Count = 0 Then ListLOMonth(Cbo_Month, CInt(Val(Me.Cbo_Year.Text)))
	End Sub

	Private Sub Cbo_Year_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Year.QueryPopUp
		If Me.Cbo_Year.Properties.Items.Count = 0 Then ListLOYear(Cbo_Year)
	End Sub

#End Region

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmFibuSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		FormIsLoaded("frmFibuSearch_LV", True)

		Try
			My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.ifrmHeight = Me.Height
			My.Settings.ifrmWidth = Me.Width

			My.Settings.strSort = Me.CboSort.Text
			My.Settings.Value_ChkNot_0 = Me.ChkNot_0.Checked
			My.Settings.Value_ChkNew_Ver = Me.ChkNewVersion.Checked
			My.Settings.Value_ChkUJList = Me.ChkUJListe.Checked
			My.Settings.Value_ChkNot_Kum = Me.ChkNot_Kum.Checked

			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Sub StartTranslation()

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
			Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)

			Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

			Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
			Me.bbiClearFields.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClearFields.Caption)
			Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

			For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.XtraTabControl1.TabPages
				tbp.Text = m_Translate.GetSafeTranslationValue(tbp.Text)
			Next


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))
		End Try

	End Sub

	Private Sub frmFibuSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And _ClsProgSetting.GetLogedUSNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase) ' GetExecutingAssembly.GetReferencedAssemblies(i).FullName)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub OnForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
			Me.Invoke(UpdateDelegate)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))

		End Try

		Try
			SetInitialFields()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try

		Me.xtabSQLQuery.PageVisible = m_InitializationData.UserData.UserNr = 1

		'If Not IsUserActionAllowed(0, 560) Then Me.mnuDesign.Visible = False
		'If Not IsUserActionAllowed(0, 556) Then Me.mnuListPrint.Visible = False

		Me.Cbo_Year.Text = If(Now.Day < 20 And Now.Month = 1, Now.Year - 1, Now.Year)
		Me.Cbo_Month.Text = If(Now.Day < 20, If(Now.Month = 1, 12, Now.Month - 1), Now.Month)

		Me.LblTimeValue.Visible = CBool(CInt(_ClsProgSetting.GetLogedUSNr()) = 1)
		Me.txt_SQL_1.Visible = CBool(CInt(_ClsProgSetting.GetLogedUSNr()) = 1)
		Me.txt_SQL_2.Visible = CBool(CInt(_ClsProgSetting.GetLogedUSNr()) = 1)

		'Me.ChkNewVersion.Visible = CBool(CInt(_ClsProgSetting.GetLogedUSNr()) = 1)
		'Me.ChkUJListe.Visible = CBool(CInt(_ClsProgSetting.GetLogedUSNr()) = 1)
		If Me.ChkNewVersion.Checked Then ClsDataDetail.IsNewVersion = True

	End Sub

	Private Sub SetInitialFields()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
				If My.Settings.frm_Location <> String.Empty Then
					Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.Message))

			End Try
			Me.ChkNewVersion.Checked = My.Settings.Value_ChkNew_Ver
			Me.ChkUJListe.Checked = My.Settings.Value_ChkUJList

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try

		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
			Me.lueMandant.Visible = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 675, m_InitializationData.MDData.MDNr)
			Me.lblMDName.Visible = Me.lueMandant.Visible

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

		Catch ex As Exception
			m_Logger.LogError(String.Format("Mandantenauswahl anzeigen: {0}", ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

	End Sub



	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	''' <param name="bForExport">ob die Liste für Export ist.</param>
	''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	''' <remarks></remarks>
	''' 
	Function GetData4Print(ByVal bForExport As Boolean, ByVal strJobInfo As String) As Boolean
		Dim bResult As Boolean = True

		Dim sSql As String = ClsDataDetail.GetSQLQuery4Print()
		If sSql = String.Empty Then
			m_Logger.LogError("Keine Suche wurde gestartet!")
			MsgBox(TranslateText("Keine Suche wurde gestartet!"), MsgBoxStyle.Exclamation, "GetData4Print_0")
			Return False
		End If

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

		Try
			Conn.Open()

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
			Try
				If Not rDbrec.HasRows Then
					cmd.Dispose()
					rDbrec.Close()

					m_Logger.LogWarning("Ich konnte leider Keine Daten finden!")
					MessageBox.Show(TranslateText("Ich konnte leider Keine Daten finden."),
													"GetData4Print_1", MessageBoxButtons.OK, MessageBoxIcon.Information)
					Return False
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetData4Print_2")
				Return False

			End Try

			If rDbrec.HasRows Then
				Me.SQL4Print = sSql
				Me.PrintJobNr = strJobInfo

				bResult = StartPrinting()


			End If
			rDbrec.Close()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Return bResult
	End Function

	Function StartPrinting() As Boolean
		Dim strFilter As String = String.Empty
		Dim bShowDesign As Boolean = IsUserActionAllowed(lueMandant.EditValue, 560)
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso bShowDesign
		Dim result As PrintResult = New PrintResult With {.Printresult = True}

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname) & vbNewLine
		strFilter &= If(m_SearchCriteria.vonmonat.Length > 0, String.Format("Zeitraum: {0}", m_SearchCriteria.vonmonat), String.Empty)
		strFilter &= If(m_SearchCriteria.vonjahr.Length > 0, String.Format(" / {0}", m_SearchCriteria.vonjahr), String.Empty)

		strFilter &= If(m_SearchCriteria.Filiale.Length > 0, String.Format("{1}Filiale: {0}", m_SearchCriteria.Filiale, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.TeilungFiliale, String.Format("{0}Teilung unterhalb der Filiale? Ja", vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.Db1Data, String.Format("{0}Benutze die DB1 Daten? Ja", vbNewLine), String.Empty)


		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLLOFibuSearchPrintSetting With {.DbConnString2Open = m_InitialData.MDData.MDDbConn,
			.SelectedMDNr = m_InitialData.MDData.MDNr,
																																											.SQL2Open = Me.SQL4Print,
																																											.JobNr2Print = Me.PrintJobNr,
																																											.ShowAsDesgin = ShowDesign,
																																											.ShowPrintBox = Me.bPrintedPages
																																										 }
		Me.bPrintedPages = False

		Dim obj As New SPS.Listing.Print.Utility.LOFibuSearchListing.ClsPrintLOFibuSearchList(_Setting)
		result = obj.PrintLOFibuSearchList(ClsDataDetail.GetSortBez,
														New List(Of String)(New String() {strFilter,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilialBez4Print()}))
		If ShowDesign Then result = New PrintResult With {.Printresult = False}


		Return result.Printresult

	End Function

#Region "Funktionen zur Menüaufbau..."

	'Private Sub OnListDesign()
	'	Dim strResult As String = "Success"

	'	Me.bPrintedPages = True
	'	If ClsDataDetail.IsNewVersion Then
	'		Dim _ClsDb As ClsDbFunc_1
	'		_ClsDb = New ClsDbFunc_1(ClsDataDetail.Conn, ClsDataDetail.GetLP(), ClsDataDetail.GetYear())

	'		If Me.Cbo_Filiale.Text.ToLower = "Alle".ToLower And Me.Cbo_Filiale.Properties.Items.Count > 1 Then
	'			For i As Integer = 1 To Me.Cbo_Filiale.Properties.Items.Count - 1
	'				Dim strFilalBez As String = Me.Cbo_Filiale.Properties.Items(i).ToString.Trim

	'				ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(Me, strFilalBez)
	'				ClsDataDetail.GetFilialBez4Print() = strFilalBez
	'				strResult = GetData4Print(False, ClsDataDetail.GetModulToPrint())
	'				If strResult.ToLower.Contains("error") Then Exit Sub

	'				Exit For

	'			Next

	'		Else
	'			Dim strFilalBez As String = Me.Cbo_Filiale.Text

	'			ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(Me, strFilalBez)
	'			ClsDataDetail.GetFilialBez4Print() = strFilalBez
	'			GetData4Print(False, ClsDataDetail.GetModulToPrint())

	'		End If

	'	Else
	'		Dim _ClsDb As ClsDbFunc
	'		_ClsDb = New ClsDbFunc

	'		If Me.Cbo_Filiale.Text.ToLower = "Alle".ToLower And Me.Cbo_Filiale.Properties.Items.Count > 1 Then
	'			For i As Integer = 1 To Me.Cbo_Filiale.Properties.Items.Count - 1
	'				Dim strFilalBez As String = Me.Cbo_Filiale.Properties.Items(i).ToString.Trim

	'				ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(strFilalBez)
	'				ClsDataDetail.GetFilialBez4Print() = strFilalBez
	'				strResult = GetData4Print(False, ClsDataDetail.GetModulToPrint())
	'				If strResult.ToLower.Contains("error") Then Exit Sub

	'				Exit For

	'			Next

	'		Else
	'			Dim strFilalBez As String = Me.Cbo_Filiale.Text

	'			ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(strFilalBez)
	'			ClsDataDetail.GetFilialBez4Print() = strFilalBez
	'			GetData4Print(False, ClsDataDetail.GetModulToPrint())

	'		End If

	'	End If

	'End Sub

	Private Sub OnListPrint()
		Dim strFilialBez As String = Me.Cbo_Filiale.Text.ToLower
		Dim strResult As Boolean = True

		' Neue Version...
		Me.bPrintedPages = True
		If ClsDataDetail.IsNewVersion Then
			Dim _ClsDb As ClsDbFunc_1
			_ClsDb = New ClsDbFunc_1(ClsDataDetail.Conn, ClsDataDetail.GetLP(), ClsDataDetail.GetYear())

			If strFilialBez = "Alle".ToLower AndAlso Me.Cbo_Filiale.Properties.Items.Count > 1 Then
				For i As Integer = 1 To Me.Cbo_Filiale.Properties.Items.Count - 1
					Dim strFilalBez As String = Me.Cbo_Filiale.Properties.Items(i).ToString.Trim

					ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(Me, strFilalBez)
					ClsDataDetail.GetFilialBez4Print() = strFilalBez
					m_SearchCriteria.Filiale = strFilalBez

					strResult = GetData4Print(False, ClsDataDetail.GetModulToPrint())
					If Not strResult Then Exit For

				Next

			Else
				Dim strFilalBez As String = Me.Cbo_Filiale.Text

				ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(Me, strFilalBez)
				ClsDataDetail.GetFilialBez4Print() = strFilalBez
				GetData4Print(False, ClsDataDetail.GetModulToPrint())

			End If

		Else
			Dim _ClsDb As ClsDbFunc
			_ClsDb = New ClsDbFunc

			If strFilialBez = "Alle".ToLower AndAlso Me.Cbo_Filiale.Properties.Items.Count > 1 Then
				For i As Integer = 1 To Me.Cbo_Filiale.Properties.Items.Count - 1
					Dim strFilalBez As String = Me.Cbo_Filiale.Properties.Items(i).ToString.Trim

					ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(strFilalBez)
					ClsDataDetail.GetFilialBez4Print() = strFilalBez
					m_SearchCriteria.Filiale = strFilalBez

					strResult = GetData4Print(False, ClsDataDetail.GetModulToPrint())
					If Not strResult Then Exit For

					Me.bPrintedPages = False

				Next

			Else
				Dim strFilalBez As String = Me.Cbo_Filiale.Text

				ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(strFilalBez)
				ClsDataDetail.GetFilialBez4Print() = strFilalBez
				GetData4Print(False, ClsDataDetail.GetModulToPrint())

			End If

		End If
		Me.bPrintedPages = False

	End Sub

	'Private Sub OnKumListDesign()
	'	Dim strFilialBez As String = Me.Cbo_Filiale.Text.ToLower
	'	Dim strResult As String = "Success"

	'	Me.bPrintedPages = True
	'	If ClsDataDetail.IsNewVersion Then
	'		Dim _ClsDb As ClsDbFunc_1
	'		_ClsDb = New ClsDbFunc_1(ClsDataDetail.Conn, ClsDataDetail.GetLP(), ClsDataDetail.GetYear())

	'		If strFilialBez.ToLower = "Alle".ToLower And Me.Cbo_Filiale.Properties.Items.Count > 1 Then
	'			For i As Integer = 1 To Me.Cbo_Filiale.Properties.Items.Count - 1
	'				Dim strFilalBez As String = Me.Cbo_Filiale.Properties.Items(i).ToString.Trim

	'				ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial_G(Me, strFilalBez)
	'				ClsDataDetail.GetFilialBez4Print() = strFilalBez
	'				strResult = GetData4Print(False, ClsDataDetail.GetModulToPrint() & ".1")
	'				If strResult.ToLower.Contains("error") Then Exit Sub

	'				Exit For

	'			Next

	'		Else
	'			Dim strFilalBez As String = Me.Cbo_Filiale.Text

	'			ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial_G(Me, strFilalBez)
	'			ClsDataDetail.GetFilialBez4Print() = strFilalBez
	'			GetData4Print(False, ClsDataDetail.GetModulToPrint() & ".1")

	'		End If

	'	End If

	'End Sub

	Private Sub OnKumListPrint()
		Dim strFilialBez As String = Me.Cbo_Filiale.Text.ToLower
		Dim strResult As Boolean = True

		' Neue Version...
		Me.bPrintedPages = True
		If ClsDataDetail.IsNewVersion Then
			Dim _ClsDb As ClsDbFunc_1
			_ClsDb = New ClsDbFunc_1(ClsDataDetail.Conn, ClsDataDetail.GetLP(), ClsDataDetail.GetYear())

			If strFilialBez = "Alle".ToLower And Me.Cbo_Filiale.Properties.Items.Count > 1 Then
				For i As Integer = 1 To Me.Cbo_Filiale.Properties.Items.Count - 1
					Dim strFilalBez As String = Me.Cbo_Filiale.Properties.Items(i).ToString.Trim

					ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial_G(Me, strFilalBez)
					ClsDataDetail.GetFilialBez4Print() = strFilalBez
					m_SearchCriteria.Filiale = strFilalBez

					strResult = GetData4Print(False, ClsDataDetail.GetModulToPrint() & ".1")
					If Not strResult Then Exit For
				Next

			Else
				Dim strFilalBez As String = Me.Cbo_Filiale.Text

				ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial_G(Me, strFilalBez)
				ClsDataDetail.GetFilialBez4Print() = strFilalBez
				m_SearchCriteria.Filiale = strFilalBez

				GetData4Print(False, ClsDataDetail.GetModulToPrint() & ".1")

			End If

		End If
		Me.bPrintedPages = False

	End Sub



	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		ClsDataDetail.IsNewVersion = Me.ChkNewVersion.Checked
		m_Logger.LogDebug(String.Format("IsNewVersion: {0} | Me.ChkNewVersion.Checked: {1}", ClsDataDetail.IsNewVersion, Me.ChkNewVersion.Checked))

		Try
			_ClsFunc.GetRPNr = 0
			_ClsFunc.GetMANr = 0
			Dim _ClsDb As New ClsDbFunc
			If Not IsNothing(ClsDataDetail.Conn) Then ClsDataDetail.Conn.Dispose()
			m_SearchCriteria = GetSearchKrieteria()

			_ClsDb.GetJobNr4Print()

			Me.txt_SQL_1.Text = String.Empty
			ClsDataDetail.GetSQLQuery() = String.Empty
			ClsDataDetail.GetSQLSortString() = String.Empty

			FormIsLoaded("frmFibuSearch_LV", True)
			Me.LblTimeValue.Text = String.Empty
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")


			GetMyQueryString()

			Me.bbiExport.Enabled = _ClsProgSetting.AllowedExportDoc(ClsDataDetail.GetModulToPrint)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

			m_UtilityUi.ShowErrorDialog(ex.Message)

		Finally

		End Try

	End Sub

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		result.mandantenname = lueMandant.Text
		result.vonjahr = Cbo_Year.EditValue
		result.vonmonat = Cbo_Month.EditValue

		result.Filiale = Cbo_Filiale.EditValue

		result.TeilungFiliale = ChkNewVersion.Checked
		result.Db1Data = ChkUJListe.Checked


		Return result

	End Function

	Function GetMyQueryString() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim strArtQuery As String = String.Empty
		Dim _ClsDb As New ClsDbFunc

		Me.bbiSearch.Enabled = False

		If Me.Cbo_Month.Text = String.Empty Then Me.Cbo_Month.Text = Now.Month
		If Me.Cbo_Year.Text = String.Empty Then Me.Cbo_Year.Text = Now.Year

		ClsDataDetail.GetLP = Me.Cbo_Month.Text
		ClsDataDetail.GetYear = Me.Cbo_Year.Text

		BackgroundWorker1.WorkerSupportsCancellation = True
		BackgroundWorker1.RunWorkerAsync()    ' Multithreading starten

		Me.txt_SQL_1.Text = ClsDataDetail.GetSQLQuery

		Return True
	End Function


#Region "Clear form"

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClearFields_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClearFields.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmFibuSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		Me.bbiSearch.Enabled = True

		ResetAllTabEntries()

		Me.CboSort.Text = strText
		Me.Cbo_Month.Text = CStr(Now.Month)
		Me.Cbo_Year.Text = CStr(Now.Year)
		Me.bbiPrint.Enabled = False

		ClsDataDetail.GetSQLQuery() = String.Empty
		ClsDataDetail.GetSQLSortString() = String.Empty

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()

		For Each ctrls In Me.Controls
			ResetControl(ctrls)
		Next

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		Try
			If con.Name.ToLower = lueMandant.Name.ToLower Then Exit Sub

			' Rekursiver Aufruf
			' Sonst Control zurücksetzen
			If TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
				Dim tb As DevExpress.XtraEditors.TextEdit = con
				tb.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
				cbo.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = con
				cbo.EditValue = Nothing

			ElseIf con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		End Try

	End Sub

#End Region



#End Region


	Private Sub InitClickHandler(ByVal ParamArray Ctrls() As Control)

		For Each Ctrl As Control In Ctrls
			AddHandler Ctrl.KeyPress, AddressOf KeyPressEvent
		Next

	End Sub

	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) ' System.EventArgs)

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

	Private Sub frmFibuSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmFibuSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub


#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Dim Time_1 As Double = System.Environment.TickCount

		CheckForIllegalCrossThreadCalls = False
		Me.LblTimeValue.Text = ""
		Me.bsiInfo.Caption = TranslateText("Nach Daten wird gesucht...")

		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		Dim strFiliale As String = Me.Cbo_Filiale.Text

		If Not ClsDataDetail.IsNewVersion Then
			Dim _ClsDb As New ClsDbFunc
			ListUJFiliale(Me.Cbo_Filiale)

			If IsNothing(ClsDataDetail.Conn) Then
				_ClsDb.GetStartSQLString(Me, ClsDataDetail.GetLP(), ClsDataDetail.GetYear(), strFiliale, Me.ChkUJListe.Checked)
			Else
				If ClsDataDetail.Conn.State <> ConnectionState.Open Then
					_ClsDb.GetStartSQLString(Me, ClsDataDetail.GetLP(), ClsDataDetail.GetYear(), strFiliale, Me.ChkUJListe.Checked)
				End If
			End If

			' SQL_Query mit Order Klausel...
			ClsDataDetail.GetSQLQuery() = _ClsDb.GetFinalQueryForOutput(Me)

		Else

			' Neue Version...
			Dim _ClsDb As ClsDbFunc_1

			_ClsDb = New ClsDbFunc_1(ClsDataDetail.Conn, ClsDataDetail.GetLP(), ClsDataDetail.GetYear())
			If IsNothing(ClsDataDetail.Conn) Then
				_ClsDb.GetStartSQLString(Me, ClsDataDetail.GetLP(), ClsDataDetail.GetYear(), strFiliale, Me.ChkUJListe.Checked)
			Else
				If ClsDataDetail.Conn.State <> ConnectionState.Open Then
					_ClsDb.GetStartSQLString(Me, ClsDataDetail.GetLP(), ClsDataDetail.GetYear(), strFiliale, Me.ChkUJListe.Checked)
				End If
			End If

			' SQL_Query mit Order Klausel...
			If Me.Cbo_Filiale.Text.ToLower = "Alle".ToLower Then
				ClsDataDetail.GetSQLQuery() = _ClsDb.GetFinalQueryForOutput(Me)
				ListUJFiliale(Me.Cbo_Filiale)

			Else
				ClsDataDetail.GetSQLQuery() = _ClsDb.GetOutputQueryForFilial(Me, Me.Cbo_Filiale.Text)
			End If

		End If
		Console.WriteLine(String.Format("Zeit für CalcDataForJournal: {0} s",
																((System.Environment.TickCount - Time_1) / 1000).ToString()))

		e.Result = True
		If bw.CancellationPending Then e.Cancel = True

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(e.Error.Message)
		Else
			If e.Cancelled = True Then
				Me.bbiSearch.Enabled = True
				MessageBox.Show(TranslateText("Aktion abgebrochen!"))

			Else
				BackgroundWorker1.CancelAsync()
				'        MessageBox.Show(e.Result.ToString())

				If Not FormIsLoaded("frmFibuSearch_LV", True) Then
					frmMyLV = New frmFibuSearch_LV(Me, ClsDataDetail.GetSQLQuery(), Me.Location.X, Me.Location.Y, Me.Height)

					frmMyLV.Show()
					Me.Select()

					Me.bsiInfo.Caption = String.Format(TranslateText("{0} Datensätze wurden aufgelistet..."), frmMyLV.RecCount)
					frmMyLV.LblState_1.Text = String.Format(TranslateText("{0} Datensätze wurden aufgelistet..."), frmMyLV.RecCount)

					Me.bbiPrint.Enabled = frmMyLV.RecCount > 0
					Me.bbiExport.Enabled = frmMyLV.RecCount > 0

					Me.bbiSearch.Enabled = True

				End If

				Me.Cbo_Filiale.Enabled = Me.Cbo_Filiale.Properties.Items.Count >= 1
				Me.txt_SQL_1.Text = ClsDataDetail.GetSQLQuery()

				If frmMyLV.RecCount > 0 Then
					Me.bbiPrint.Enabled = True
					Me.bbiExport.Enabled = True

					CreatePrintPopupMenu()
					CreateExportPopupMenu()
				End If
			End If


			PlaySound(0)
		End If


	End Sub

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim bAllowedDesign = IsUserActionAllowed(m_InitialData.UserData.UserNr, 560)

		Dim liMnu As New List(Of String) From {"Liste drucken#PrintList",
																					 If(Cbo_Filiale.Properties.Items.Count > 1, "Kumulativliste drucken#PrintKumList", "#")
																					}
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
				bshowMnu = Not String.IsNullOrWhiteSpace(myValue(0))
				'If myValue(1).ToString.ToLower.Contains("Design".ToLower) Then bshowMnu = IsUserActionAllowed(0, 560) Else bshowMnu = myValue(0).ToString <> String.Empty

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

					popupMenu.AddItem(itm)
					'If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.SQL4Print = ClsDataDetail.GetSQLQuery4Print()
		Me.PrintJobNr = ClsDataDetail.GetModulToPrint()

		Try
			Select Case e.Item.Name.ToUpper
				Case "PrintList".ToUpper
					OnListPrint()

				'Case "printdesign".ToUpper
				'	OnListDesign()


				Case "PrintKumList".ToUpper
					OnKumListPrint()

					'Case "printKumdesign".ToUpper
					'	OnKumListDesign()


				Case Else
					Exit Sub

			End Select


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick

		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiExport.DropDownControl
		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Daten in CSV- / TXT exportieren...#CSV",
																					 If(IsModulLicenceOK("sesam"), "Daten in SESAM exportieren...#SESAM", "#"),
																					 If(IsModulLicenceOK("abacus"), "Daten in ABACUS exportieren...#ABACUS", "#"),
																					 If(IsModulLicenceOK("cresus"), "Daten in CRESUS exportieren...#CRESUS", "#")}

		If Not IsUserAllowed4DocExport(ClsDataDetail.GetModulToPrint) Then Exit Sub
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
				bshowMnu = Not myValue(0) = String.Empty
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
		Dim strSQL As String = ClsDataDetail.GetSQLQuery4Print()
		Dim result As String = "success"

		m_Logger.LogDebug(String.Format("MnuName: ", UCase(e.Item.Name.ToUpper)))

		If ClsDataDetail.IsNewVersion Then
			Dim _ClsDb As ClsDbFunc_1
			_ClsDb = New ClsDbFunc_1(ClsDataDetail.Conn, ClsDataDetail.GetLP(), ClsDataDetail.GetYear())

			If Me.Cbo_Filiale.Text.ToLower = "Alle".ToLower And Me.Cbo_Filiale.Properties.Items.Count > 1 Then
				For i As Integer = 1 To Me.Cbo_Filiale.Properties.Items.Count - 1
					Dim strFilalBez As String = Me.Cbo_Filiale.Properties.Items(i).ToString.Trim

					ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(Me, strFilalBez)
					ClsDataDetail.GetFilialBez4Print() = strFilalBez
					'result = GetData4Print(True, False, ClsDataDetail.GetModulToPrint())
					'If result.ToLower.Contains("error") Then Exit Sub

					Exit For

				Next

			Else
				Dim strFilalBez As String = Me.Cbo_Filiale.Text

				ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(Me, strFilalBez)
				ClsDataDetail.GetFilialBez4Print() = strFilalBez
				'GetData4Print(True, False, ClsDataDetail.GetModulToPrint())

			End If

		Else
			Dim _ClsDb As ClsDbFunc
			_ClsDb = New ClsDbFunc

			If Me.Cbo_Filiale.Text.ToLower = "Alle".ToLower And Me.Cbo_Filiale.Properties.Items.Count > 1 Then
				For i As Integer = 1 To Me.Cbo_Filiale.Properties.Items.Count - 1
					Dim strFilalBez As String = Me.Cbo_Filiale.Properties.Items(i).ToString.Trim

					ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(strFilalBez)
					ClsDataDetail.GetFilialBez4Print() = strFilalBez
					'result = GetData4Print(True, False, ClsDataDetail.GetModulToPrint())
					'If result.ToLower.Contains("error") Then Exit Sub

					Exit For

				Next

			Else
				Dim strFilalBez As String = Me.Cbo_Filiale.Text

				ClsDataDetail.GetSQLQuery4Print() = _ClsDb.GetOutputQueryForFilial(strFilalBez)
				ClsDataDetail.GetFilialBez4Print() = strFilalBez
				'GetData4Print(True, False, ClsDataDetail.GetModulToPrint())

			End If

		End If

		Select Case UCase(e.Item.Name.ToUpper)
			Case "TXT".ToUpper, "CSV".ToUpper
				Dim ExportThread As New Thread(AddressOf StartExportModul)
				ExportThread.Name = "ExportLOFibuListToCSV"
				ExportThread.Start()

			Case "SESAM".ToUpper
				StartSesamModul()
				'Dim ExportThread As New Thread(AddressOf StartSesamModul)
				'ExportThread.Name = "ExportLOFibuListToSESAM"
				'ExportThread.Start()

			Case "Abacus".ToUpper
				StartAbacusModul()

				'Dim ExportThread As New Thread(AddressOf StartAbacusModul)
				'ExportThread.Name = "ExportLOFibuListToABACUS"
				'ExportThread.Start()

			Case "Cresus".ToUpper
				StartCresusModul()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																			 .SQL2Open = ClsDataDetail.GetSQLQuery4Print()
																																			 }
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		_Setting.ModulName = "LOFibubelegToCSV"
		obj.ExportCSVFromLOFibuListing(Me.SQL4Print)

	End Sub

	Sub StartSesamModul()
		m_Logger.LogDebug("Sesam-Schnittstelle wird gestartet...")
		Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																				.ModulName = "SesamLO".ToLower,
																																				.SQL2Open = ClsDataDetail.GetSQLQuery4Print,
																																				.SelectedMonth = ClsDataDetail.GetLP,
																																				.SelectedYear = ClsDataDetail.GetYear
																																			 }
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_setting)

		Try
			obj.ShowSesamForm()

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.Message)

		End Try

	End Sub

	Sub StartAbacusModul()
		m_Logger.LogDebug("Abacus-Schnittstelle wird gestartet...")
		Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																				.ModulName = "AbaLO".ToLower,
																																				.SQL2Open = ClsDataDetail.GetSQLQuery4Print,
																																				.SelectedMonth = ClsDataDetail.GetLP,
																																				.SelectedYear = ClsDataDetail.GetYear
																																			 }
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_setting)

		Try
			obj.ShowAbacusForm()

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.Message)

		End Try

	End Sub


	Sub StartCresusModul()
		m_Logger.LogDebug("Cresus-Schnittstelle wird gestartet...")
		Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																				.ModulName = "CresusLO".ToLower,
																																				.SQL2Open = ClsDataDetail.GetSQLQuery4Print,
																																				.SelectedMonth = ClsDataDetail.GetLP,
																																				.SelectedYear = ClsDataDetail.GetYear
																																			 }
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_setting)

		Try
			obj.ShowCresusForm()

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.Message)

		End Try

	End Sub


#End Region


	Private Sub Cbo_Month_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Month.TextChanged,
																																																Cbo_Year.TextChanged
		Dim _ClsDb As New ClsDbFunc

		If Not IsNothing(ClsDataDetail.Conn) Then
			Me.txt_SQL_1.Text = String.Empty
			Me.txt_SQL_2.Text = String.Empty

			Me.bbiSearch.Enabled = True
			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

			_ClsDb.DeleteAllRecinDb()
			ClsDataDetail.Conn.Dispose()

			Trace.WriteLine("Cbo_Month_TextChanged")
		End If

	End Sub

	Private Sub ChkUJListe_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkUJListe.CheckedChanged
		Dim _ClsDb As New ClsDbFunc

		'Me.Cbo_Filiale.Visible = Me.ChkUJListe.Checked
		'Me.LblFilial.Visible = Me.ChkUJListe.Checked

		If Not IsNothing(ClsDataDetail.Conn) Then
			Me.txt_SQL_1.Text = String.Empty
			Me.txt_SQL_2.Text = String.Empty

			Me.bbiSearch.Enabled = True
			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

			_ClsDb.DeleteAllRecinDb()
			ClsDataDetail.Conn.Dispose()
			Trace.WriteLine("ChkNot_0_CheckedChanged")
		End If

	End Sub

	Private Sub ChkNot_0_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkNot_0.CheckedChanged,
																																																		ChkNot_Kum.CheckedChanged,
																																																		ChkUJListe.CheckedChanged,
																																																		ChkNewVersion.CheckedChanged
		Dim _ClsDb As New ClsDbFunc

		ClsDataDetail.IsNewVersion = Me.ChkNewVersion.Checked
		If Not IsNothing(ClsDataDetail.Conn) Then
			Me.txt_SQL_1.Text = String.Empty
			Me.txt_SQL_2.Text = String.Empty

			Me.bbiSearch.Enabled = True
			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

			_ClsDb.DeleteAllRecinDb()
			ClsDataDetail.Conn.Dispose()
			ClsDataDetail.Conn = Nothing
			Trace.WriteLine("ChkNot_0_CheckedChanged")
		End If

	End Sub

	'Private Sub bbisearch_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs)

	'	Me.bbiPrint.Enabled = Me.bbisearch.Enabled
	'	Me.bbiExport.Enabled = Me.bbisearch.Enabled
	'	Me.btnEmptyFields.Enabled = Me.bbisearch.Enabled
	'	Trace.WriteLine("bbisearch_EnabledChanged")

	'End Sub

	Private Sub Cbo_Filiale_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Filiale.KeyPress, Cbo_Month.KeyPress, Cbo_Year.KeyPress

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.StackTrace & vbNewLine & ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	'#Region "Mandanten Daten auflisten..."

	'  ' Mandantendaten...
	'  Private Sub LookUpEdit15_EditValueChanged(sender As Object, _
	'                               e As System.EventArgs) Handles LookUpEdit15.EditValueChanged
	'    Dim test As Object = Me.LookUpEdit15.GetSelectedDataRow
	'    Dim currow As DataRowView = TryCast(LookUpEdit15.GetSelectedDataRow(), DataRowView)
	'    If Not currow Is Nothing Then
	'      Me.GetMDDbName = currow("DbName").ToString()
	'      Me.GetMDGuid = currow("MDGuid").ToString()

	'      ClsDataDetail.GetSelectedDbName = Me.GetMDDbName

	'    End If

	'  End Sub

	'  Private Sub LookUpEdit15_QueryCloseUp(sender As Object, _
	'                                       e As System.ComponentModel.CancelEventArgs) Handles LookUpEdit15.QueryCloseUp

	'    Dim test As Object = Me.LookUpEdit15.GetSelectedDataRow
	'    Dim currow As DataRowView = TryCast(LookUpEdit15.GetSelectedDataRow(), DataRowView)
	'    If Not currow Is Nothing Then
	'      Me.GetMDDbName = currow("DbName").ToString()
	'      Me.GetMDGuid = currow("MDGuid").ToString()
	'    End If


	'  End Sub

	'  Private Sub LookUpEdit15_QueryPopUp(sender As Object, _
	'                                     e As System.ComponentModel.CancelEventArgs) Handles LookUpEdit15.QueryPopUp
	'    Me.LookUpEdit15.Properties.Columns.Clear()

	'    Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString())
	'    Conn.Open()

	'    Dim strQuery As String = "[Cockpit. Get All Allowed MDData]"
	'    Dim adapter As New SqlDataAdapter()
	'    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
	'    adapter.SelectCommand.Connection = Conn
	'    Dim rFoundedrec As New DataSet

	'    adapter.SelectCommand.CommandText = strQuery
	'    adapter.SelectCommand.CommandType = Global.System.Data.CommandType.Text
	'    adapter.Fill(rFoundedrec, "MDData")
	'    Dim dt As DataTable = rFoundedrec.Tables(0)
	'    Me.LookUpEdit15.Properties.DataSource = dt


	'    LookUpEdit15.Properties.DisplayMember = "MDName"
	'    LookUpEdit15.Properties.ValueMember = "DBName" ' "ZHDFullName"

	'    Dim Col0 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("AllGuid", "AllGuid", 0)
	'    Dim Col1 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("MDName", _
	'                                                                     _ClsProgSetting.TranslateText("Mandantenname"), 300)
	'    Dim Col2 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("MDGuid", "MDGuid", 0)
	'    Dim Col3 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("DbName", "DbName", 0)

	'    '    Col2.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending
	'    Col0.Visible = False
	'    Col2.Visible = False
	'    Col3.Visible = False
	'    LookUpEdit15.Properties.Columns.Add(Col0)
	'    LookUpEdit15.Properties.Columns.Add(Col1)
	'    LookUpEdit15.Properties.Columns.Add(Col2)
	'    LookUpEdit15.Properties.Columns.Add(Col3)

	'    LookUpEdit15.Properties.BestFitMode = BestFitMode.BestFitResizePopup
	'    LookUpEdit15.Properties.ForceInitialize()

	'  End Sub

	'#End Region



End Class

